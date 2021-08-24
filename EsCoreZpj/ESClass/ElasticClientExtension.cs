using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsCoreZpj.ESClass
{
    /// <summary>
    /// ElasticClient 扩展类
    /// </summary>
    public static class ElasticClientExtension
    {
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elasticClient"></param>
        /// <param name="indexName"></param>
        /// <param name="numberOfShards"></param>
        /// <param name="numberOfReplicas"></param>
        /// <returns></returns>
        public static bool CreateIndex<T>(this ElasticClient elasticClient, string indexName = "", int numberOfShards = 5, int numberOfReplicas = 1) where T : class
        {

            if (string.IsNullOrWhiteSpace(indexName))
            {
                indexName = typeof(T).Name;
            }

            if (elasticClient.Indices.Exists(indexName).Exists)
            {
                return false;
            }
            else
            {
                var indexState = new IndexState()
                {
                    Settings = new IndexSettings()
                    {
                        NumberOfReplicas = numberOfReplicas,
                        NumberOfShards = numberOfShards,
                    },
                };
                var response = elasticClient.Indices.Create(indexName, p => p.InitializeUsing(indexState).Map<T>(p => p.AutoMap()));
                return response.Acknowledged;
            }
        }
    }
}
