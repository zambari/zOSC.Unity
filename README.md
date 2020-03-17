# zOSC.Unity v.2

First off, what is zOSC? 
Well, obviously it is an OSC library. Technically it could be called a wrapper around another library, UnityOSC (c) 2012 by Jorge Garcia Martin

it started off as a mod, but it quickly moved passed. While there is still a lot of UnityOSC burried underneath, quite a lot has been added and original library got improved in many ways (namely, all of the terrible type comparisons allocating strings for each compare have been replaced with statically typed code)


## Add OSC Control with a single line


Lets imagine you have a very important method to call remotely:

    void MyMethodIWantToRemotelyFeed(float f);
    {
        Debug.Log("yay, we received "+f);
    }

With zOSC binding (pretty much) any osc command to your code is as simple as adding a single line:

    void Start()
    {
        zOSC.Bind(this,"/myOSCAddress",MyMethodIWantToRemotelyFeed);
    }

Tutorial ends here. Your code is ready. The Bind method is overloaded in waay to many ways to be considered sane, you can bind Vector3s, Quaternions, Blobs (aka byte[]) etc. There is even a special type for transferring over structs (and marshalling them both ways).

The *this* reference points to a MonoBehaviour which you bind. While this isn't strickly necessary, it lets me handle the exception in a nice way that lets you have a trace of what happend. Otherwise, if anything triggered via OSC would throw an exception, it would be more difficult to debug (the trace would be lost, as there can be multiple listeners bind to the same address).

Binding is resolved by matching both name and types of both incoming message and methods in your code. It may sound ridiculous but actually makes binding events across the network totally rad.

## What about sending OSC?

Again, this is a single line line voyage


     zOSC.BroadcastOSC("/address",0,4f);


For more complex messages we fall back to UnityOSC
    
    
    OSCMessage message=new OSCMessage("/address");
    message.Append("whatever");
    message.Append(new byte[6]);
    zOSC.BroadcastOSC(message);

## Is it stable?

Despite the gloomy look of some parts of the code (this is one of my first attempts at writing general purpose code for Unity), zOSC has been tried and tested a lot. 

The only situations in which it fails, is when your OS blocks the traffic (I am looking at you, WINDOWS FIREWALL).

In some cases the "127.0.0.1" address for loopback would eat packets, try using an IP

## Will it work on android?

Yes it does, between windows, mac (haven't checked on a mac in a while), and android devices. I have not tried linux nor ios.

## Will it work over the internet?

Generally speaking, no, it wont. OSC packets are essentially UDP packets, and unless you try funny things, it will only work if your devices are in the same local network (LAN or WiFi).

In case of doubt - try doing a PING x.x.x.x check to verify that the devices can exchange any packets at all.

## Downsides?

The biggest downside is that its one heck of an ugly Singleton. Currently you can only target one server and open one port. 

Also you may find that bits of code are a little bit too convoluted - there was quite an ambitious plan to support getters (Func<>) binding, ACKS, auto detection, and domain routing. At the moment most of it is either not used or commented out.

## Compatibility?

NET 2.0 Subset, Unity 5.6 (You'll probably want 2017.4 to open the demo scene but you should be all right with just unity 5.6).

## Changes in 2.0

If, by any chance, you were a user of zOSC, I have to sadly inform that there are breaking API changes with version 2.0. Most notably, parameter order changed for Bind, was (Monobehaviour, Method, string), is (Monobehaviour, string, Method). I like it so much better that way, but didn't want to break existing code. 
With a lot of improvements (finally got rid of most of those scary string allocations happening tons of times per message with UnityOSC)