# CodeSample: Generic ObjectPool
Generic Object Pool system wrapping Unity3D's inbuilt ObjectPool<T>

This allows you to quickly setup the needed events with some quick properties to help common use cases, while enabling the ability to over-ride for special use cases.

This repo contains the core scripts, and a .unitypackage you can download to load into Unity. Coded in Unity 2021.3.6f1 but should work for any 2021.1 or later version (as that was when ObjectPool<T0> was introduced)

In the package there is a test scene setup, this scene will:

* Creates 20 objects and de-actives each one under >Enemies in the editor hierachy
* When you press F it will enable an object under >Enemies or create a new one under >Enemies
* When you hit G it will disable the object