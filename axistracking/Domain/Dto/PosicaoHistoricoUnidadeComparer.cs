using System;
using System.Collections.Generic;

namespace axistracking.Domain.Dto
{
	public class PosicaoHistoricoUnidadeComparer: IEqualityComparer<PosicaoHistorico>
	{
		// Products are equal if their names and product numbers are equal.
		public bool Equals(PosicaoHistorico x, PosicaoHistorico y)
		{

			//Check whether the compared objects reference the same data.
			if (Object.ReferenceEquals(x, y)) return true;

			//Check whether any of the compared objects is null.
			if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
				return false;

			//Check whether the products' properties are equal.
			return x.IdUnidadeRastreada == y.IdUnidadeRastreada;
		}

		// If Equals() returns true for a pair of objects 
		// then GetHashCode() must return the same value for these objects.

		public int GetHashCode(PosicaoHistorico product)
		{
			////Check whether the object is null
			//if (Object.ReferenceEquals(product, null)) return 0;

			////Get hash code for the Name field if it is not null.
			//int hashProductName = product.Identificacao == null ? 0 : product.Identificacao.GetHashCode();

			////Get hash code for the Code field.
			//int hashProductCode = product.IdPosicao.GetHashCode();

			//Calculate the hash code for the product.
			return product.GetHashCode();
		}

	}
}
