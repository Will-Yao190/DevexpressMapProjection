using DevExpress.Map;
using DevExpress.Map.Native;
using DevExpress.XtraMap;
using DevExpress.XtraMap.Native;
using System;

namespace MapProjections
{
    public class NAFLambertAzimuthalEqualAreaProjection : LambertAzimuthalEqualAreaProjectionBase {
        internal NAFLambertAzimuthalEqualAreaProjection() : base() { 
            OriginLatitude = 90.0;
        }
        protected override ProjectionBaseCore CreateProjectionCore() {
            return new NAFLambertAzimuthalEqualAreaProjectionCoreBase(GeoPointFactory.Instance);
        }
        public override string ToString()
        {
            return $"({nameof(NAFLambertAzimuthalEqualAreaProjection)})";
        }
    }
}
