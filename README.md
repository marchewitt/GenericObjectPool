# CodeSample: Generic ObjectPool
Generic Object Pool system wrapping Unity3D's inbuilt ObjectPool which is an effective stack implementation under the hood that they optimize occasionally.<T>

This package of code allows you to quickly setup the needed events with some quick properties to help common use cases, while enabling the ability to over-ride for special use cases.


This repo contains:
* the core scripts for review in github
* a .unitypackage you can download to load into any Unity project with a quick double click


## ObjectPoolExample.UnityPackage
Once you download the package, simply double click while your Unity scene is open, the package contains all needed code/prefabs and a test scene pre-setup. 

This package was coded in Unity 2021.3.6f1 but should work for any Unity 2021.1 or later version as that was when ObjectPool<T0> was introduced.

This scene on running will:
* Creates 20 objects and de-actives each one under >Enemies in the editor hierachy
* When you press F it will enable an object under >Enemies or create a new one under >Enemies
* When you hit G it will disable the object
