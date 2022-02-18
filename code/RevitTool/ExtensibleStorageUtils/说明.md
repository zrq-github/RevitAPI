# 关于ExtensibleStorageUtility的使用说明

在这个命令空间中，封装的是针对RevitAPI中扩展数据的使用

## 重要提要：

- **对扩展数据的读写必须放在事务中**
  因为数据更新设计的原因，在升级完成的时候，会再次写入扩展。
- **所有数据扩展类，必须继承 IExtendStorageBase**，这里建议使用**UpdateStorage**
  在顶层实现中，在更新的时候，调用的接口，如果没有，升级不了。并且一些其他操作可能出错。
- 因为支持数据更新，并且是在读写的完成数据更新，所以对扩展数据的读写建议全部放在事务中去；

## SchemaWrapperTools

命令空间是是模式创建，封装了关于revit对于扩展数据的创建等操作。
因为是全量操作，内容很多，如果遇到revitAPI改变，再这里面修改就可以了。相当于给revitAPI的二次封装

### 默认创建的表的格式

StorageVersion（string）: 整个扩展数据表结构的版本
StorageString（string）：目前来说，没什么用
StorageList（List<string>）：目前来说，没什么用
StorageDictionary（Dictionary<key : string, value : string> ）: 
key: 类的名字
value: 类数据的序列号，目前采用的是json序列号，**注意类必须支持json序列号**

## ExtensibleStorageUtility

### IUpdateStorage

提供数据class的升级操作，数据源如果想要升级，必须有接口

### UpdateStorage

对IUpdateStorage的基本实现

### IExtendStorageBase

统一扩展数据源接口，一般来说数据源实现这样的接口就行了

包含区分版本的字段Version
包括IUpdateStorage接口

### ExtendStorageManage

abstract：抽象类

如果要使用扩展表，请继承这个类，并完成主要的实现方式，就可以调用。

public void SetDictionary<T>(Element storageElement, T Object)
保存数据

public T GetDictionary<T>(Element storageElement)
得到数据

具体的使用方式，后续说明。

## 设计原理

### SchemaWrapperTools：

相当于对RevitAPI的扩展数据的使用的二次封装，避免因为直接使用原生API导致Revit版本更替的时候，大范围更改

### IUpdateStorage:

提供数据更新机制，如果继承了此接口，在ExtendStorageManage中会主动调用更新事件，按照自定义的更新方法，更新到标注位结束，并且更新完毕后，就会主动写入到扩展数据中去，所以要求必须开启事务

### IExtendStorageBase：

每个扩展数据都必须继承的接口。
设计目的是为了，当有统一需求的时候，可以统一增加新的接口，因为数据更新机制是强制的，此接口已经继承了IUpdateStorage

### 谈谈跟以前封装的对比：

总得来讲，此次的更新优化，和原来相比并没有本质的改变。
重新设计后，初始代码更加复杂了，但是可扩展性有更加的提升。
针对部分痛点进行说明。

- （旧）将表的创建方式，开放了出来了。由开发者自己控制对表的创建。据了解，revit有一个比较严重的问题，如果新增了字段，会被判断会不同的表，导致以前的数据全部消失，就没办法升级替换了。
  （新）统一表的创建，提供三个字段，string,list<string>,Dictionary<string,string>，应该是满足了所有的需求，如果还有比较特殊的需求，建议拿出来讨论一下后，再新类继承前面的基类，单独提供新增字段的接口
- （旧）散装升级机制，由开发者完全控件，就我目前所观测到的写法，并不支持跨类的数据升级。
  （新）封装升级机制，在进行读写的时候，会自动调用更新函数，升级完成后，会强制写入更新后的数据
- （旧）按照以前的设计，表字段只有一个string，所以所有数据信息都必须再使用一个类进行封装。
  当遇到需要对小部分数据进行修改的及其麻烦，不好维护, 并且容易对这个类进行滥用。
  （新）不同再对各种结构统一封装，因为字典的原因，需要什么样的数据，就取什么样的数据。
  灵活存储，能很好的隔离不同的数据结构。

## 使用例子

### 创建一个表

建议按照功能常见，相关的功能使用同一个表格

```C#
internal class ExtendStorageTable : ExtendStorageManage
{
    public override Guid SchemaGuid { get; } = new Guid("1FE5BD8A-D624-40FF-BBE0-2EF7755F7817");
    protected override AccessLevel ReadLevel { get; } = AccessLevel.Public;
    protected override AccessLevel WriteLevle { get; } = AccessLevel.Public;
    protected override string StorageVersion { get; } = "1.0";
    protected override string StorageName { get; } = nameof(ExtendStorageTable);
    protected override string ApplicationId { get; } = RevitApplication.ApplicationId;
    
        // 建议主动提供每个数据类的 Get/Set方法, 在里面再调用父类的Set/Get
        // 一是方便自己知道，这个表中存了那些数据
        // 二是方面通过引用，找到指定的数据类在那些地方被修改了

        internal bool SetStoragePerson(Element element, Storage_Person elevation)
        {
            SetDictionary(element, elevation);
            return true;
        }

        internal Storage_Person GetStoragePerson(Element element)
        {
            Storage_Person elevation = GetDictionary<Storage_Person>(element);
            if (elevation == null)
            {
                // 记日志
            }
            return elevation;
        }
}
```

### 扩展数据的完整结构

注意时效性！

#### 基本数据类接口

```C#
public interface IExtendStorageBase : IUpdateStorage
{

}
```

#### 支持数据类更新的接口

```C#
    public interface IUpdateStorage
    {
        /// <summary>
        /// 版本号
        /// </summary>
        /// <remarks>默认应该为1.0</remarks>
        string CurVersion { get; set; }

        /// <summary>
        /// 更新状态
        /// </summary>
        UpdataResult UpdataState { get; set; }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <remarks>默认返回false</remarks>
        UpdataResult UpdateData();

        /// <summary>
        /// 跨类更新
        /// </summary>
        /// <remarks>
        /// 从当前类更新到其他类型
        /// </remarks>
        object UpdataNewClass { get; set; }
    }
```

#### 接口的默认实现

```c#
    /// <summary>
    /// IUpdateStorage的基本实现
    /// </summary>
    public class UpdateStorage : IExtendStorageBase
    {
        public virtual string Version { get; set; } = "1.0";
        public virtual object UpdataNewClass { get; set; } = null;

        public virtual UpdataResult UpdateData(Element element)
        {
            throw new NotImplementedException();
        }
    }
```

#### 数据类的基本用法

```C#
internal class Storage_Person : UpdateStorage
{
    public Storage_Person()
    {
        Version = "1.0";
        PersonDescription = $"this is a {nameof(Storage_Person)} Class";
    }

    public string PersonDescription { get; set; }
    public override string Version { get; set; }
    public override UpdataResult UpdateData(Element element)
    {
        throw new NotImplementedException();
    }
}
```

### 使用方式

```C#
Element ele = null;
ExtendStorageTable extendStorageTable = new ExtendStorageTable();
// 读扩展
Storage_Person storage_Person = extendStorageTable.GetStoragePerson(ele);
if (storage_Person != null)
{
}
// 写扩展
extendStorageTable.SetDictionary(ele, storage_Person);

```

我这里调用的就是自己写的Get/Set方法，至于是返回null还是新new一个出来，就看个人习惯

