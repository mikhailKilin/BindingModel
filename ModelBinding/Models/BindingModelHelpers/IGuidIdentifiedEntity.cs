﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBinding.Models.BindingModelHelpers
{
    public interface IGuidIdentifiedEntity
    {
        #region Свойства

        /// <summary>
        /// Уникальный идентификатор сущности.
        /// </summary>
        Guid Id { get; set; }

        #endregion
    }
    public interface INamedEntity : IGuidIdentifiedEntity
    {
        #region Свойства

        /// <summary>
        /// Наименование сущности.
        /// </summary>
        string Name { get; set; }

        #endregion
    }
}
