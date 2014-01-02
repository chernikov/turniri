using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Mappers;

namespace turniri.Controllers
{
    public interface IModelMapperController
    {
        IMapper ModelMapper { get; }
    }
}
