using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.Map;
using DevExpress.XtraMap;
using DevExpress.XtraMap.Native;

namespace MapProjections
{
    public partial class Form1 : Form
    {
        private string _dataPath;
        private MapItemStyle _oceanStyle = new MapItemStyle
        {
            Fill = Color.White,
            Stroke = Color.White,
            StrokeWidth = 1
        };
        private MapItemStyle _landStyle = new MapItemStyle
        {
            Fill = Color.FromArgb(234, 244, 255), //.LightGreen;
            StrokeWidth = 0
        };
        private MapItemStyle _riverStyle = new MapItemStyle
        {
            Fill = Color.White,
            Stroke = Color.White,
            StrokeWidth = 1
        };
        private MapItemStyle _lakeStyle = new MapItemStyle
        {
            Fill = Color.White,
            Stroke = Color.White,
            StrokeWidth = 1
        };
        private MapItemStyle _coastlineStyle = new MapItemStyle
        {
            Fill = Color.Transparent,
            Stroke = Color.FromArgb(0, 128, 192), //.Yellow;
            StrokeWidth = 1
        };
        private MapItemStyle _countryStyle = new MapItemStyle
        {
            Fill = Color.Transparent,
            Stroke = Color.FromArgb(0, 128, 192), //.Yellow;
            StrokeWidth = 1
        };
        private MapItemStyle _stateStyle = new MapItemStyle
        {
            Fill = Color.Transparent,
            Stroke = Color.FromArgb(0, 128, 192), //.Yellow;
            StrokeWidth = 1
        };

        private ShapefileDataAdapter _oceans = null;
        private ShapefileDataAdapter _rivers = null;
        private ShapefileDataAdapter _lakes = null;
        private ShapefileDataAdapter _coastlines = null;
        private ShapefileDataAdapter _continents = null;
        private ShapefileDataAdapter _countries = null;
        private ShapefileDataAdapter _countriesSecond = null;
        private ShapefileDataAdapter _states = null;

        List<ProjectionBase> mapProjections = new List<ProjectionBase>() {
            new BraunStereographicProjection(),
            new EllipticalMercatorProjection(),
            new EqualAreaProjection(),
            new EquidistantProjection(),
            new EquirectangularProjection(),
            new KavrayskiyProjection(),
            new LambertCylindricalEqualAreaProjection(),
            new MillerProjection(),
            new SinusoidalProjection(),
            new SphericalMercatorProjection(),
            new NAFLambertAzimuthalEqualAreaProjection()
        };

        GeoMapCoordinateSystem CoordinateSystem { get { return mapControl.CoordinateSystem as GeoMapCoordinateSystem; } }

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateGrid();
            lbProjection.DataSource = mapProjections;
            lbProjection.SetSelected(0, true);
        }
        private ShapefileDataAdapter CreateShapefileDataAdapter(string relativePath)
        {
            if (DesignMode)
                return null;
            Uri baseUri = new Uri(System.Reflection.Assembly.GetEntryAssembly().Location);
            Uri uri = new Uri(baseUri, _dataPath);
            Uri shapeUri = new Uri(uri, relativePath);

            // Create an adapter to load data from shapefile.
            ShapefileDataAdapter adapter = new ShapefileDataAdapter
            {
                FileUri = shapeUri,
                SourceCoordinateSystem = new GeoSourceCoordinateSystem()
            };
            return adapter;
        }

        private static VectorItemsLayer CreateVectorLayer(ShapefileDataAdapter source, MapItemStyle style, int zIndex, bool bVisible = true, string label = "")
        {
            VectorItemsLayer vectorItemsLayer = new VectorItemsLayer
            {
                Data = source,
                // Enable shape titles
                ShapeTitlesVisibility = VisibilityMode.Auto,
                // To display this value as a shape title, attribute name goes in braces.
                ShapeTitlesPattern = $"{{{label}}}",
                EnableHighlighting = false,
                EnableSelection = false
            };
            MapElementStyleBase.MergeStyles(vectorItemsLayer.ItemStyle, style, vectorItemsLayer.ItemStyle);
            vectorItemsLayer.ZIndex = zIndex;
            vectorItemsLayer.Visible = bVisible;
            return vectorItemsLayer;
        }

        private void Data_ItemsLoaded(object sender, ItemsLoadedEventArgs e) {
            e.Items[6].Visible = false;
        }

        private void MapControl_MouseMove(object sender, MouseEventArgs e) {
           CoordPoint coordPoint = mapControl.ScreenPointToCoordPoint(e.Location);
            MapUnit unit = mapControl.ScreenPointToMapUnit(e.Location.ToMapPoint());
            this.Text = coordPoint.ToString() + "   " + unit.ToString();
        }

        private void lbProjection_SelectedIndexChanged(object sender, EventArgs e) 
        {
            CoordinateSystem.Projection = lbProjection.SelectedValue as ProjectionBase;

            bool isNAF = lbProjection.SelectedValue is NAFLambertAzimuthalEqualAreaProjection;
            _dataPath = isNAF ? @"..\..\Data\low_polar_north\" : @"..\..\Data\Low\";
            mapControl.MinZoomLevel = isNAF ? 0.5D : 2D;
            CoordinateSystem.CircularScrollingMode = isNAF ? DevExpress.XtraMap.CircularScrollingMode.None : DevExpress.XtraMap.CircularScrollingMode.TilesAndVectorItems;
            
            // Dispose all existing shapefiles so we can put in the new ones
            _oceans?.Dispose();
            _rivers?.Dispose();
            _lakes?.Dispose();
            _coastlines?.Dispose();
            _countries?.Dispose();
            _countriesSecond?.Dispose();
            // Load in the new shapefiles
            _oceans = CreateShapefileDataAdapter(@"ocean.shp");
            _rivers = CreateShapefileDataAdapter(@"rivers.shp");
            _lakes = CreateShapefileDataAdapter(@"lakes.shp");
            _coastlines = CreateShapefileDataAdapter(@"coastline.shp");
            _countries = CreateShapefileDataAdapter(@"countries.shp");
            _countriesSecond = CreateShapefileDataAdapter(@"countries.shp");
            mapControl.Layers.Clear();
            mapControl.Layers.Add(vectorItemsLayer1);
            mapControl.Layers.Add(CreateVectorLayer(_coastlines, _coastlineStyle, 97));
            mapControl.Layers.Add(CreateVectorLayer(_oceans, _oceanStyle, 99));
            mapControl.Layers.Add(CreateVectorLayer(_countries, _landStyle, 98));
            mapControl.Layers.Add(CreateVectorLayer(_rivers, _riverStyle, 96));
            mapControl.Layers.Add(CreateVectorLayer(_lakes, _lakeStyle, 95));

            mapControl.MouseMove += MapControl_MouseMove;
        }

        void GenerateGrid() {
            vectorItemsLayer1.ZIndex = 1;
            Color gridColor = Color.Red;// FromArgb(50, 255, 255, 255);
            GenerateLongitudes(gridColor);
        }
        void GenerateLongitudes(Color gridColor) {
            for(double i = -180; i <= 180; i += 45) {
                CoordPointCollection points = new CoordPointCollection();
                for(int y = 0; y <= 90; y++)
                    points.Add(new GeoPoint(y, i));
                MapPolyline line = new MapPolyline() {
                    Points = points,
                    StrokeWidth = 1,
                    Stroke = gridColor,
                    IsGeodesic = false
                };
                line.TitleOptions.Pattern = i.ToString();
                GridData.Items.Add(line);
            }
        }

        /// <summary>
        /// This method is used to remove all shapes below 45N, split the shapes across 45N, and export to a new shape file.
        /// </summary>
        /// <param name="vectorItemsLayer"></param>
        /// <param name="fileName"></param>
        private void SaveN45Shapes(VectorItemsLayer vectorItemsLayer, string fileName)
        {
            foreach (MapPath mapPath in vectorItemsLayer.Data.Items)
            {
                for (int j = mapPath.Segments.Count - 1; j >= 0; j--)
                {
                    CoordPointCollection newPoints = new CoordPointCollection();
                    MapPathSegment sg = mapPath.Segments[j];
                    bool allAbove45N = true;
                    bool hasPartAbove45N = false;
                    CoordPointCollection temPoints = new CoordPointCollection();
                    foreach (GeoPoint point in sg.Points)
                    {
                        if (point.Latitude > 45 && !hasPartAbove45N)
                        {
                            hasPartAbove45N = true;
                        }
                        else if (point.Latitude < 45 && allAbove45N)
                        {
                            allAbove45N = false;
                        }
                        temPoints.Add(point);
                    }
                    // the shape is all above 45N, don't need to do anything
                    if (allAbove45N)
                    {
                        continue;
                    }
                    // the shape is all below 45N, don't need to do anything
                    if (!hasPartAbove45N)
                    {
                        mapPath.Segments.Remove(sg);
                        continue;
                    }

                    // the value of last index is same as value of first index, just remove it for following operation
                    temPoints.RemoveAt(temPoints.Count - 1);
                    int lastIndex = temPoints.Count - 1;
                    for (int z = 0; z <= lastIndex; z++)
                    {
                        // add point above 45N
                        if (((GeoPoint)temPoints[z]).Latitude >= 45)
                        {
                            newPoints.Add((GeoPoint)temPoints[z]);
                        }
                        else
                        {
                            //get previous and next points which are above 45N
                            int previousIndex = z, nextIndex = z;
                            if (z == 0)
                            {
                                for (int x = lastIndex; x > 0; x--)
                                {
                                    if (((GeoPoint)temPoints[x]).Latitude >= 45)
                                    {
                                        previousIndex = x;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = z - 1; i >= 0; i--)
                                {
                                    if (((GeoPoint)temPoints[i]).Latitude >= 45)
                                    {
                                        previousIndex = i;
                                        break;
                                    }
                                    if (i == 0)
                                    {
                                        for (int a = lastIndex; a > z; a--)
                                        {
                                            if (((GeoPoint)temPoints[a]).Latitude >= 45)
                                            {
                                                previousIndex = a;
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }

                            if (z == lastIndex)
                            {
                                for (int x = 0; x < lastIndex; x++)
                                {
                                    if (((GeoPoint)temPoints[x]).Latitude >= 45)
                                    {
                                        nextIndex = x;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = z + 1; i <= lastIndex; i++)
                                {
                                    if (((GeoPoint)temPoints[i]).Latitude >= 45)
                                    {
                                        nextIndex = i;
                                        break;
                                    }
                                    if (i == lastIndex)
                                    {
                                        for (int y = 0; y < z; y++)
                                        {
                                            if (((GeoPoint)temPoints[y]).Latitude >= 45)
                                            {
                                                nextIndex = y;
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }

                            // only one point above 45N, get its new left and right side point
                            if (previousIndex == nextIndex)
                            {
                                int leftIndex, rightIndex;
                                if (previousIndex == 0)
                                    leftIndex = lastIndex;
                                else
                                    leftIndex = previousIndex - 1;

                                if (nextIndex == lastIndex)
                                    rightIndex = 0;
                                else
                                    rightIndex = nextIndex + 1;

                                double midLat = ((GeoPoint)temPoints[previousIndex]).Latitude;
                                double midLon = ((GeoPoint)temPoints[previousIndex]).Longitude;
                                double leftLat = ((GeoPoint)temPoints[leftIndex]).Latitude;
                                double rightLat = ((GeoPoint)temPoints[rightIndex]).Latitude;
                                double leftLon = ((GeoPoint)temPoints[leftIndex]).Longitude;
                                double rightLon = ((GeoPoint)temPoints[rightIndex]).Longitude;
                                double newLonLeft = leftLon + (midLon - leftLon) * ((45 - leftLat) / (midLat - leftLat));
                                double newLonRight = rightLon + (midLon - rightLon) * ((45 - rightLat) / (midLat - rightLat));
                                newPoints.Add(new GeoPoint(45, newLonLeft));
                                newPoints.Add(temPoints[previousIndex]);
                                newPoints.Add(new GeoPoint(45, newLonRight));
                                break;
                            }
                            else
                            {
                                double midLat = ((GeoPoint)temPoints[z]).Latitude;
                                double midLon = ((GeoPoint)temPoints[z]).Longitude;
                                double leftLat = ((GeoPoint)temPoints[previousIndex]).Latitude;
                                double leftLon = ((GeoPoint)temPoints[previousIndex]).Longitude;
                                double rightLat = ((GeoPoint)temPoints[nextIndex]).Latitude;
                                double rightLon = ((GeoPoint)temPoints[nextIndex]).Longitude;
                                // if this point's previous and next point are both above 45N, just get left and right middle points
                                if ((z == 0 && previousIndex == lastIndex || z != 0 && z - 1 == previousIndex) &&
                                    (z == lastIndex && nextIndex == 0 || z != lastIndex && z + 1 == nextIndex))
                                {
                                    newPoints.Add(new GeoPoint(45, leftLon + (midLon - leftLon) * ((45 - leftLat) / (midLat - leftLat))));
                                    newPoints.Add(new GeoPoint(45, rightLon + (midLon - rightLon) * ((45 - rightLat) / (midLat - rightLat))));
                                }
                                // if this point's previous is above 45N, just get left middle points
                                else if (z == 0 && previousIndex == lastIndex || z != 0 && z - 1 == previousIndex)
                                {
                                    newPoints.Add(new GeoPoint(45, leftLon + (midLon - leftLon) * ((45 - leftLat) / (midLat - leftLat))));
                                }
                                // if this point's next point is above 45N, just get right middle points
                                else if (z == lastIndex && nextIndex == 0 || z != lastIndex && z + 1 == nextIndex)
                                {
                                    newPoints.Add(new GeoPoint(45, rightLon + (midLon - rightLon) * ((45 - rightLat) / (midLat - rightLat))));
                                }
                                else
                                {
                                    double secondLeftLat = previousIndex == lastIndex ? ((GeoPoint)temPoints[0]).Latitude :
                                        ((GeoPoint)temPoints[previousIndex + 1]).Latitude;
                                    double secondLeftLon = previousIndex == lastIndex ? ((GeoPoint)temPoints[0]).Longitude :
                                        ((GeoPoint)temPoints[previousIndex + 1]).Longitude;
                                    double secondRightLat = nextIndex == 0 ? ((GeoPoint)temPoints[lastIndex]).Latitude :
                                        ((GeoPoint)temPoints[nextIndex - 1]).Latitude;
                                    double secondRightLon = nextIndex == 0 ? ((GeoPoint)temPoints[lastIndex]).Longitude :
                                        ((GeoPoint)temPoints[nextIndex - 1]).Longitude;
                                    //we need to get the new left and right middle points that generated, check if current point is between them
                                    double secondLeftNewLon = leftLon + (secondLeftLon - leftLon) * ((45 - leftLat) / (secondLeftLat - leftLat));
                                    double secondRightNewLon = rightLon + (secondRightLon - rightLon) * ((45 - rightLat) / (secondRightLat - rightLat));
                                    if (secondLeftNewLon >= secondRightNewLon)
                                    {
                                        if (secondRightNewLon <= ((GeoPoint)temPoints[z]).Longitude && ((GeoPoint)temPoints[z]).Longitude <= secondLeftNewLon)
                                        {
                                            newPoints.Add(new GeoPoint(45, ((GeoPoint)temPoints[z]).Longitude));
                                        }
                                    }
                                    else
                                    {
                                        if (secondLeftNewLon <= ((GeoPoint)temPoints[z]).Longitude && ((GeoPoint)temPoints[z]).Longitude <= secondRightNewLon)
                                        {
                                            newPoints.Add(new GeoPoint(45, ((GeoPoint)temPoints[z]).Longitude));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //not sure if we need to remove it or not, maybe no difference
                    if (newPoints.Count == 0)
                        mapPath.Segments.Remove(sg);
                    else
                    {
                        newPoints.Add(newPoints[0]);
                        sg.Points.Clear();
                        sg.Points.AddRange(newPoints);
                    }
                }
            }

            vectorItemsLayer.ExportToShp(System.Reflection.Assembly.GetEntryAssembly().Location + @"..\..\Data\low_polar_north", new ShpExportOptions
            {
                ExportToDbf = true,
                ExportToShx = true,
                ShapeType = ShapeType.Polygon
            });
        }

        /// <summary>
        /// This method is used to remove all points below 45N from a line, and export to a new shape file.
        /// </summary>
        /// <param name="vectorItemsLayer"></param>
        /// <param name="fileName"></param>
        private void SaveN45Lines(VectorItemsLayer vectorItemsLayer, string fileName)
        {
            foreach (MapPath mapPath in vectorItemsLayer.Data.Items)
            {
                for (int j = mapPath.Segments.Count - 1; j >= 0; j--)
                {
                    CoordPointCollection newPoints = new CoordPointCollection();
                    MapPathSegment sg = mapPath.Segments[j];
                    foreach (var point in sg.Points)
                    {
                        if (((GeoPoint)point).Latitude >= 45)
                        {
                            newPoints.Add(point);
                        }
                    }

                    //not sure if we need to remove it or not, maybe no difference
                    if (newPoints.Count == 0)
                        mapPath.Segments.Remove(sg);
                    else
                    {
                        sg.Points.Clear();
                        sg.Points.AddRange(newPoints);
                    }
                }
            }

            vectorItemsLayer.ExportToShp(System.Reflection.Assembly.GetEntryAssembly().Location + @"..\..\Data\low_polar_north", new ShpExportOptions
            {
                ExportToDbf = true,
                ExportToShx = true,
                ShapeType = ShapeType.Polygon
            });
        }
    }
}
