using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsCoreZpj.ESClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EsCoreZpj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private AddressContext _AddressContext;
        public AddressController(AddressContext context)
        {
            _AddressContext = context;
        }
        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="address"></param>
        [HttpPost("添加地址")]
        public void AddAddress(List<Address> addressList)
        {
            if (addressList == null || addressList.Count < 1)
            {
                return;
            }
            _AddressContext.InsertMany(addressList);
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        [HttpPost("deleteAddress")]
        public void DeleteAdress(string id)
        {
            _AddressContext.DeleteById(id);
        }
        /// <summary>
        /// 获取所有与地址
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllAddress")]
        public List<Address> GetAllAddress()
        {
            return _AddressContext.GetAllAddresses();
        }
        /// <summary>
        /// 获取地址总数
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAddressTotalCount")]
        public long GetAddressTotalCount()
        {
            return _AddressContext.GetTotalCount();
        }

        /// <summary>
        /// 分页获取（可以进一步封装查询条件）
        /// </summary>
        /// <param name="province"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost("getAddressByProvince")]
        public List<Address> GetAddressByProvince(string province, int pageIndex, int pageSize)
        {
            return _AddressContext.GetAddresses(province, pageIndex, pageSize);
        }

    }
}
