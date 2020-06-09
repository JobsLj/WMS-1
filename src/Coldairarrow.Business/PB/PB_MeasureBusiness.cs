﻿using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_MeasureBusiness : BaseBusiness<PB_Measure>, IPB_MeasureBusiness, ITransientDependency
    {
        public PB_MeasureBusiness(IRepository repository, IServiceProvider svcProvider)
            : base(repository)
        {
            _ServiceProvider = svcProvider;
        }
        readonly IServiceProvider _ServiceProvider;
        #region 外部接口

        public async Task<PageResult<PB_Measure>> GetDataListAsync(PageInput<PB_MeasureQM> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Measure>();
            var search = input.Search;

            //筛选
            if (!search.Keyword.IsNullOrEmpty())
                where = where.And(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PB_Measure> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(PB_Measure data)
        {
            if (data.Code.IsNullOrEmpty())
            {
                var codeSvc = _ServiceProvider.GetRequiredService<IPB_BarCodeTypeBusiness>();
                data.Code = await codeSvc.Generate("PB_Measure");
            }
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(PB_Measure data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}