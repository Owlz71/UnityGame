# 7 23宇宙射击

## 制作

- [x] 鼠标控制飞机

  - [x] 飞机跟随鼠标转
    - [x] 飞机
    - [x] 飞机受到鼠标位置的某个力移动
    - [x] 飞机箭头跟随鼠标位置转向

- [x] 飞机自动射击

  - [x] 一直发射子弹
  - [x]  射击方向

- [x] 子弹

  - [x] 碰到敌机进行判定
    - [x] 子弹销毁
    - [x] 敌机销毁
    - [x] 记录分数
  - [x] 配到边界销毁

- [x] 敌机体

  - [x] 路线随机直线

  - [x] 受到攻击就消失

- [x] 游戏管理

  - [x] 随机生成敌机





## BUG

- [x] 子弹穿过边界碰撞时而有效，时而失效

  - [x] 临时用外部Box作为碰撞

  - [ ] 如何用Border碰撞实现？

- [x] 随机生成的敌人直接向外面跑去了，怎么调到中间
  - [ ] 用了向中心移动，再做偏移的算法



## 记录

### 如何设计随机运动方向生成？

```c#
两种方案
        // 随机生成一个移动方向（朝向屏幕中心的随机点）

        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 randomTarget = Camera.main.ScreenToWorldPoint(center + new Vector3(
            Random.Range(-100, 100),  // x 方向随机偏移像素
            Random.Range(-100, 100),
            0
        ));
        randomTarget.z = 0;
        Vector2 dir = (randomTarget - enemy.transform.position).normalized;

        // 随机生成一个移动方向
        float angle = random.range(0f, 360f);
        dir = new vector2(mathf.cos(angle * mathf.deg2rad), mathf.sin(angle * mathf.deg2rad));
```



### 如何随机位置生成敌人？

​	在边界外生成

​	获取摄像机参数

​		Camera.main.ScreenToWorldPoint 得到摄像机世界坐标

```c#
Vector3 screenBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
Vector3 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
float minX = screenBottomLeft.x;
float maxX = screenTopRight.x;
float minY = screenBottomLeft.y;
float maxY = screenTopRight.y;
    等价于
float minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
float maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
float minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
float maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
```

```c#
核心  spawnPos = new Vector2(Random.Range(minX, maxX), maxY + extraDistance);


	   int side = Random.Range(0, 4); // 0=上, 1=下, 2=左, 3=右
        Vector2 spawnPos = Vector2.zero;

        switch (side)
        {
            case 0: // 上
                spawnPos = new Vector2(Random.Range(minX, maxX), maxY + extraDistance);
                break;
            case 1: // 下
                spawnPos = new Vector2(Random.Range(minX, maxX), minY - extraDistance);
                break;
            case 2: // 左
                spawnPos = new Vector2(minX - extraDistance, Random.Range(minY, maxY));
                break;
            case 3: // 右
                spawnPos = new Vector2(maxX + extraDistance, Random.Range(minY, maxY));
                break;
        }
        return spawnPos;
```



### Gamemanager无法继承monobehavier ，导致Instantiate无法用

这是 Unity 的一个经典坑：**`GameManager` 往往设计成单例，但继承 `MonoBehaviour` 才能用 `Instantiate`、`StartCoroutine`、`Invoke` 等 Unity API**。如果你写成纯 C# 类（不继承 `MonoBehaviour`），`Instantiate` 当然用不了。

------

**为什么 GameManager 不能用 `Instantiate`？**

`Instantiate` 是 Unity 提供的 **静态方法**，但它的运行逻辑依赖于 Unity 引擎的对象系统（必须在主线程、场景上下文中执行）。
 如果你的 `GameManager` 不是挂在某个 `GameObject` 上的 `MonoBehaviour`，那它无法参与 Unity 的生命周期，很多 API 都直接报错。



**方案 1：让 GameManager 继承 MonoBehaviour**

最简单的办法就是让 `GameManager` 变成一个 **Unity 对象单例**

```
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject enemyPrefab;

    private void Awake()
```

**方案 2：保持 GameManager 不继承 MonoBehaviour**

如果你硬要让 `GameManager` 作为纯 C# 类，可以让它持有一个 **MonoBehaviour 执行器** 来代理 `Instantiate`。

```

```





### static class GameManager跟private static GameManager _instance 啥区别

#### 1. `static class GameManager`

- **整个类是静态的**，不能实例化，不能 new，不能继承 `MonoBehaviour`。
- 类里面的所有成员（字段、方法）都必须是 `static`。
- 只适合放工具方法、纯逻辑代码，不适合需要状态管理和生命周期的东西。
- 不能挂到 GameObject 上，也没生命周期函数（比如 `Start()`、`Update()`）。
- 调用时直接写：`GameManager.SomeMethod()`。

------

2. #### `private static GameManager _instance;`（单例模式）

- 这是经典单例设计的一部分，意味着 **`GameManager` 是一个普通类（通常继承 `MonoBehaviour`），但只允许有一个实例存在**。
- `_instance` 是指向这个唯一实例的静态引用，方便在全局访问。
- 这个类可以有状态、非静态成员，有生命周期函数（`Start`, `Update`），还能挂在 GameObject 上。
- 调用时一般写：`GameManager.Instance.SomeMethod()`。



#### 什么时候用哪个？

- **工具方法+纯逻辑无状态**，用 `static class`，轻量方便，比如 `Mathf`、`Debug` 类那样。
- **游戏管理、状态保存、挂场景上管理对象**，用单例模式，确保有实例，也能用 `Instantiate`、`Coroutine` 等 Unity 特性。

静态方法访问的成员变量也得是静态的



### ArgumentNullException: Value cannot be null. Parameter name: _unity_self UnityEditor.SerializedObject.FindProperty (System.String propertyPath) 问题

####  **脚本或组件上的序列化字段异常**

- 你可能给某个字段赋值了无效（null）或者字段被意外删除。
- Inspector 试图读取一个不存在或空的 SerializedProperty 导致报错。

**解决：**

- 找到报错相关的脚本组件，检查所有序列化字段（public 或 `[SerializeField]` 私有字段）有没有异常，比如挂了空引用。
- 关闭 Unity，再重新打开项目，清理缓存。
- 有时候删掉出错的组件重新添加可以解决。
- 

### Unity 坑 ArgumentNullException: Value cannot be null.Parameter name: _unity_self

删掉Library文件夹 ，但是会导致场景清空



### 找不到文本 The type or namespace name 'Text' could not be found (are you missing a using directive or an assembly reference?)

导入 using UnityEngine.UI;
