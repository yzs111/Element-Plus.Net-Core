using AutoMapper;
using CW_ToyShopping.Common.Cache;
using CW_ToyShopping.Common.Helpers;
using CW_ToyShopping.Common.Helpers.Output;
using CW_ToyShopping.Common.Helpers.Page;
using CW_ToyShopping.Enity.PublicModels;
using CW_ToyShopping.IRepository;
using CW_ToyShopping.IRepository.PublicIRepository;
using CW_ToyShopping.IService.PublicIService;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping.Service.PublicService
{
   public class AuthorService: IAuthorService
    {
        private IRepositoryWrapper _repositoryWrapper { get; }
        private IMapper _mapper { get; }
        private ICache _cache { get; }

        public AuthorService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ICache cache)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IResponseOutput> GetAuthirList()
        {

            //#region 缓存时间设置
            //MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            //options.AbsoluteExpiration = DateTime.Now.AddMinutes(10);
            //options.Priority = CacheItemPriority.Normal;
            //#endregion

            var Items = await _repositoryWrapper.Author.GetAllAsync();

            var AuthorList = _mapper.Map<List<AuthirDto>>(Items);

            var data = new PageOutput<AuthirDto>
            {
                Total = AuthorList.Count,
                List = AuthorList,
            };
            return ResponseOutput.Ok(data);
        }

        public async Task<DataTable> PrintAuthir(List<DicModel> dic)
        {

            var Items = await _repositoryWrapper.Author.GetAllAsync();

            var aa = DataTableHelper<Author>.MondelConverToDatable(Items, dic);

           /* var data = new PageOutput<AuthirDto>
            {
                Total = AuthorList.Count,
                List = AuthorList,
            };*/
            return aa;
        }

        public async Task<IResponseOutput> CreateAuthor(AuthirDto authirDto) 
        {
            var author = _mapper.Map<Author>(authirDto);

            _repositoryWrapper.Author.Create(author);

            var Istrue = await _repositoryWrapper.Author.SaveAsync();

            if (Istrue) {
                return ResponseOutput.Ok("新增成功");
            }
            return ResponseOutput.NotOk("新增失败");
        }

        public async Task<IResponseOutput> UpdateAuthor(AuthirDto authirDto)
        {
 
            var AuthorModel = await _repositoryWrapper.Author.GetByIdAsync(authirDto.AuthorID);

            
            // 找不到该条数据
            if (AuthorModel == null) {
                return ResponseOutput.NotOk("修改失败,修改的数据不存在");
            }
            //var author = _mapper.Map<Author>(authirDto);            

            AuthorModel.BIRTHDATA = authirDto.BIRTHDATA;
            AuthorModel.NAME = authirDto.NAME;
            AuthorModel.EMAIL = authirDto.EMAIL;

            _repositoryWrapper.Author.Update(AuthorModel);

            var Istrue = await _repositoryWrapper.Author.SaveAsync();

            if (Istrue)
            {
                return ResponseOutput.Ok("修改成功");
            }
            return ResponseOutput.NotOk("修改失败");
        }
    }
}
