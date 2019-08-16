using CodeVerse.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeVerse.Logic.Maps
{
    public interface IMapGenerator
    {
        List<Entity> Generate();
    }
}
