using ALMS.Model;
using AutoMapper;
using Elsa.NNF.Data.ORM;

namespace ALMS.Core
{
    public static class MappingExtensions
    {
        private static IMapper _mapper;
        public static void Configure(IMapper mapper)
        {
            _mapper = mapper;
        }
        public static TDtoObject ConvertTo<TDtoObject>(this IEntity entity)
        {
            return _mapper.Map<TDtoObject>(entity);
        }
        public static TEntityObject ConvertTo<TEntityObject>(this IDtoEntity entity)
        {
            return _mapper.Map<TEntityObject>(entity);
        }
    }
}