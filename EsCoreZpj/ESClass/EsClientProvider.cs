using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsCoreZpj.ESClass
{
    /// <summary>
    /// ElasticClient提供者
    /// </summary>
    public class EsClientProvider : IEsClientProvider
    {
        private readonly IOptions<EsConfig> _EsConfig;
        public EsClientProvider(IOptions<EsConfig> esConfig)
        {
            _EsConfig = esConfig;
        }
        /// <summary>
        /// 获取elastic client
        /// </summary>
        /// <returns></returns>
        public ElasticClient GetClient()
        {
            if (_EsConfig == null || _EsConfig.Value == null || _EsConfig.Value.Urls == null || _EsConfig.Value.Urls.Count < 1)
            {
                throw new Exception("urls can not be null");
            }
            return GetClient(_EsConfig.Value.Urls.ToArray(), "");
        }
        /// <summary>
        /// 指定index获取ElasticClient
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public ElasticClient GetClient(string indexName)
        {
            if (_EsConfig == null || _EsConfig.Value == null || _EsConfig.Value.Urls == null || _EsConfig.Value.Urls.Count < 1)
            {
                throw new Exception("urls can not be null");
            }
            return GetClient(_EsConfig.Value.Urls.ToArray(), indexName);
        }


        /// <summary>
        /// 根据url获取ElasticClient
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaultIndex"></param>
        /// <returns></returns>
        private ElasticClient GetClient(string url, string defaultIndex = "")
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("es 地址不可为空");
            }
            var uri = new Uri(url);
            var connectionSetting = new ConnectionSettings(uri);
            if (!string.IsNullOrWhiteSpace(url))
            {
                connectionSetting.DefaultIndex(defaultIndex);
            }
            connectionSetting.BasicAuthentication("elastic", "123456"); //设置账号密码
            return new ElasticClient(connectionSetting);
        }
        /// <summary>
        /// 根据urls获取ElasticClient
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="defaultIndex"></param>
        /// <returns></returns>
        private ElasticClient GetClient(string[] urls, string defaultIndex = "")
        {
            if (urls == null || urls.Length < 1)
            {
                throw new Exception("urls can not be null");
            }
            var uris = urls.Select(p => new Uri(p)).ToArray();
            var connectionPool = new SniffingConnectionPool(uris);
            var connectionSetting = new ConnectionSettings(connectionPool);
            if (!string.IsNullOrWhiteSpace(defaultIndex))
            {
                connectionSetting.DefaultIndex(defaultIndex);
            }
            connectionSetting.BasicAuthentication("elastic", "123456"); //设置账号密码
            return new ElasticClient(connectionSetting);
        }
    }
}
