namespace ReadyGamerOne.Common
{
    public interface IPoolable<T>
        where T:class
    {
        /// <summary>
        /// 对象池为空时，加载资源
        /// </summary>
        /// <returns></returns>
        T OnInit();
        /// <summary>
        /// 对象池回收Person时调用
        /// </summary>
        void OnRecycleToPool();
        /// <summary>
        /// 对象池加载Person时调用
        /// </summary>
        void OnGetFromPool();
    }
}