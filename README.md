# Unity Collider Support
Made in Unity 6000.0.13f1 and didnt tested in other environments.

By design (hate that word), Unity **only** sends collision messages to the hierarchy of Rigidbody. And also Unity does not sends `OnTriggerExit()` message to self whenever other collider is disabled or destroyed. This package aims to solve that problem.

I did my best to not hit the performance while supporting anything i thought of. So i say, package creates zero garbage but iterates through dictionaries by twice. Your help may make this package faster by sending a pull request.

[ChildCollisionNotifier](https://github.com/RiskyWilhelm/UnityColliderSupport/blob/7ebfaf3837882e891f275c39a6f14e3c29f977cb/Assets/MoveAnywhereYouWant/MonoBehaviours/ChildCollisionNotifier.cs)
----
To send collision messages to children, put this to Rigidbody hierarchy and it will notify to listeners. This is an example hierarchy:
+ Rigidbody with ChildCollisionNotifier
  + Any MonoBehaviour [ICollisionEnterListener](https://github.com/RiskyWilhelm/UnityColliderSupport/blob/7ebfaf3837882e891f275c39a6f14e3c29f977cb/Assets/MoveAnywhereYouWant/Shared/ICollisionEnterListener.cs)
  + Any MonoBehaviour [ICollisionStayListener](https://github.com/RiskyWilhelm/UnityColliderSupport/blob/7ebfaf3837882e891f275c39a6f14e3c29f977cb/Assets/MoveAnywhereYouWant/Shared/ICollisionStayListener.cs)
  + Any MonoBehaviour [ICollisionExitListener](https://github.com/RiskyWilhelm/UnityColliderSupport/blob/7ebfaf3837882e891f275c39a6f14e3c29f977cb/Assets/MoveAnywhereYouWant/Shared/ICollisionExitListener.cs)
  + Any MonoBehaviour [ICollisionExitDisabledListener](https://github.com/RiskyWilhelm/UnityColliderSupport/blob/7ebfaf3837882e891f275c39a6f14e3c29f977cb/Assets/MoveAnywhereYouWant/Shared/ICollisionExitDisabledListener.cs) - **Other** collider must have [DisabledColliderNotifier](https://github.com/RiskyWilhelm/UnityColliderSupport/blob/main/README.md#disabledcollidernotifier) at its hierarchy.

[DisabledColliderListenerTrigger](https://github.com/RiskyWilhelm/UnityColliderSupport/blob/7ebfaf3837882e891f275c39a6f14e3c29f977cb/Assets/MoveAnywhereYouWant/MonoBehaviours/DisabledColliderListenerTrigger.cs)
----
Allows self game object to take [ITriggerExitDisabledListener](https://github.com/RiskyWilhelm/UnityColliderSupport/blob/7ebfaf3837882e891f275c39a6f14e3c29f977cb/Assets/MoveAnywhereYouWant/Shared/ITriggerExitDisabledListener.cs) message by listening to [DisabledColliderNotifier](https://github.com/RiskyWilhelm/UnityColliderSupport/blob/main/README.md#disabledcollidernotifier)(s). You can think that component as a trigger. If it is not there, it wont get notified by [DisabledColliderNotifier](https://github.com/RiskyWilhelm/UnityColliderSupport/blob/main/README.md#disabledcollidernotifier).

[DisabledColliderNotifier](https://github.com/RiskyWilhelm/UnityColliderSupport/blob/7ebfaf3837882e891f275c39a6f14e3c29f977cb/Assets/MoveAnywhereYouWant/MonoBehaviours/DisabledColliderNotifier.cs)
----
Notifies whenever a collider gets disabled. As a downside, destroying a collider in the hierarchy is not allowed and notifier wont send message. Only sends whenever collider's GameObject gets destroyed.

### TODO
- DisabledColliderNotifier can detect destroyed colliders before it gets destroyed
