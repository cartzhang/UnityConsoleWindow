/// <summary>
/// 生成单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonProvider<T> where T : class, new()
{
    SingletonProvider()
    {
    }

    public static T Instance
    {
        get
        {
            if (SingletonCreator.Instance == null)
            {
                SingletonCreator.Instance = new T();
            }
            return SingletonCreator.Instance;
        }
    }

    public static void Clear()
    {
        SingletonCreator.Instance = null;
    }

    class SingletonCreator
    {
        static SingletonCreator()
        {
        }

        internal static T Instance = new T();
    }
}