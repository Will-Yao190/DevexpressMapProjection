using DevExpress.Map.Native;
using DevExpress.XtraMap;
using DevExpress.XtraMap.Native;

namespace MapProjections
{
    public class NAFLambertAzimuthalEqualAreaProjection : Etrs89LambertAzimuthalEqualAreaProjection
    {
        internal NAFLambertAzimuthalEqualAreaProjection() : base()
        {
        }

        protected override ProjectionBaseCore CreateProjectionCore()
        {
            return new NAFLambertAzimuthalEqualAreaProjectionCoreBase(GeoPointFactory.Instance);
        }

        public override string ToString()
        {
            return $"({nameof(NAFLambertAzimuthalEqualAreaProjection)})";
        }
    }
}
