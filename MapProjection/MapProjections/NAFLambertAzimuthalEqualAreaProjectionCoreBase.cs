using DevExpress.Map;
using DevExpress.Map.Native;

namespace MapProjections
{
    public class NAFLambertAzimuthalEqualAreaProjectionCoreBase : ETRS89LAEACore
    {
        public NAFLambertAzimuthalEqualAreaProjectionCoreBase(CoordPointFactory pointFactory) : base(pointFactory)
        {
            base.LatitudeOfOrigin = 90.0d;
            base.CentralMeridian = 0.0d;
        }

        public override MapBounds BoundingBox => new MapBounds(-180.0, 45.0, 180.0, 90.0);

        protected override EllipsoidCore Ellipsoid => EllipsoidCore.WGS84;

        public override double MinLat => 45.0d;

        public override double LatitudeOfOrigin
        {
            get => 90.0d;
            set { return; }
        }
        public override double CentralMeridian
        {
            get => 0.0d;
            set { return; }
        }
    }
}
