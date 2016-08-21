# RGB LED sample

This project demonstrates how to manipulate the colors of a RGB LED with a Raspberry Pi and Windows 10 IoT.

## Introduction

The app consists of a Universal Windows App (UAP) with a slider-based UI to manipulate the colors Red, Green and Blue. 

There are also two animations for switching and interpolating (transitioning) between the colors.

## Project structure

The project is based on the Model-View-ViewModel (MVVM) pattern, with view models and services. Platform-dependent code is separated into interfaces and implementation.

This project uses the MVVM Light Toolkit.

To enable PWM on Raspberry Pi, this project references the _Microsoft.IoT.Lightning_ package.

### Services

The project implements the following services:
* PwmService _- to modulate the signals for the RGB LED._
* GpioService _- not used, but available anyway._

## Setup

For starters, to make PWM work on RPi, you have to do some additional setup on your device. 

[Here](http://www.codeproject.com/Articles/1095762/How-to-Fade-an-LED-with-PWM-in-Windows-IoT) is an excellent guide.

### Pin configuration

By default the color pins, as defined in ```MainViewModel.cs```, are:

```
    private const int RED_PIN = 5;
    private const int GREEN_PIN = 6;
    private const int BLUE_PIN = 13;
```


If you have a _common cathode RGB LED_, then the longest pin has to be connected to ```GND``` (ground).

If you have a _common anode RGB LED_, then the longest pin has to be connected to ```5V```.

You also have to add the compilation symbol ```COMMON_ANODE``` to your project. _(This has been added as default)_