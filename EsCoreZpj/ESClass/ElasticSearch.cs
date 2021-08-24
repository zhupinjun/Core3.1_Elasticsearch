using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsCoreZpj.ESClass
{
    /// <summary>
    /// 接口限定
    /// </summary>
    public interface IBaseEsContext { }
    /// <summary>
    /// es操作基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseEsContext<T> : IBaseEsContext where T : class
    {
        protected IEsClientProvider _EsClientProvider;
        public abstract string IndexName { get; }
        public BaseEsContext(IEsClientProvider provider)
        {
            _EsClientProvider = provider;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
        public bool InsertMany(List<T> tList)
        {
            var client = _EsClientProvider.GetClient(IndexName);
            if (!client.Indices.Exists(IndexName).Exists)
            {
                client.CreateIndex<T>(IndexName);
            }
            var response = client.IndexMany(tList);
            //var response = client.Bulk(p=>p.Index(IndexName).IndexMany(tList));
            return response.IsValid;
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <returns></returns>
        public long GetTotalCount()
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var search = new SearchDescriptor<T>().MatchAll(); //指定查询字段 .Source(p => p.Includes(x => x.Field("Id")));
            var response = client.Search<T>(search);
            return response.Total;
        }
        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <returns></returns>
        public bool DeleteById(string id)
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var response = client.Delete<T>(id);
            return response.IsValid;
        }

    }
}
