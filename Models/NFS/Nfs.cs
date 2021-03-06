﻿using MapEditor.Attributes;
using MapEditor.Extends;
using MapEditor.Modules;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace MapEditor.Models
{
    public class Nfs : INotifyPropertyChanged
    {
        private string sign = "nFlavor Script\0\0";
        private int version = 2;
        
        private List<Location> respawns = new List<Location>();
        private List<PropScriptInfo> props = new List<PropScriptInfo>();

        [ReadOnly(true)]
        [PropertyGridBrowsable(true)]
        [DisplayName("Signature")]
        [Category("Information")]
        public string Sign
        {
            get { return sign; }
            set
            {
                sign = value;
                OnPropertyChanged("Sign");
            }
        }

        [ReadOnly(true)]
        [PropertyGridBrowsable(true)]
        [DisplayName("Version")]
        [Category("Information")]
        public int Version
        {
            get { return version; }
            set
            {
                version = value;
                OnPropertyChanged("Version");
            }
        }

        [DisplayName("Respawns")]
        [PropertyGridBrowsable(true)]
        [Editor(typeof(CollectionEditorExtends), typeof(UITypeEditor))]
        public List<Location> Respawns
        {
            get { return respawns; }
            set
            {
                respawns = value;
                OnPropertyChanged("Respawns");
            }
        }

        [DisplayName("NPC Props")]
        [PropertyGridBrowsable(true)]
        [Editor(typeof(CollectionEditorExtends), typeof(UITypeEditor))]
        public List<PropScriptInfo> Props
        {
            get { return props; }
            set
            {
                props = value;
                OnPropertyChanged("Props");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Nfs()
        {
            Instance = this;
        }

        public int GetScriptCount()
        {
            int i = 0;
            foreach (var location in Respawns)
            {
                if (location.Scripts.Count != 0) i++;
            }
            return i;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static Nfs Instance = null;
    }

    public class Location : INotifyPropertyChanged
    {
        private int left = 0;
        private int top = 0;
        private int right = 0;
        private int bottom = 0;
        private string description = "";
        private List<Function> script = new  List<Function>();
		
        [PropertyGridBrowsable(false)]
        public int Left
        {
            get { return left; }
            set
            {
                left = value;
                OnPropertyChanged("Left");
            }
        }
		
        [PropertyGridBrowsable(false)]
        public int Top
        {
            get { return top; }
            set
            {
                top = value;
                OnPropertyChanged("Top");
            }
        }
		
        [PropertyGridBrowsable(false)]
        public int Right
        {
            get { return right; }
            set
            {
                right = value;
                OnPropertyChanged("Right");
            }
        }
		
        [PropertyGridBrowsable(false)]
        public int Bottom
        {
            get { return bottom; }
            set
            {
                bottom = value;
                OnPropertyChanged("Bottom");
            }
		}

		[DisplayName("X")]
		[Category("Coordonnate 1")]
		[PropertyGridBrowsable(true)]
		public int mLeft
		{
			get { return (MapWorker.Instance.X * 16128) + (left * Global.tileLenght); }
			set
			{
				var part = MapWorker.Instance.X * 16128;
				var partmax = MapWorker.Instance.X * 16128 + 16128;

				if (value < part || value > partmax)
				{
					Left = 0;
					return;
				}

				Left = (value - part) / Global.tileLenght;
				OnPropertyChanged("mLeft");
			}
		}

		[DisplayName("Y")]
		[Category("Coordonnate 1")]
		[PropertyGridBrowsable(true)]
		public int mTop
		{
			get { return (MapWorker.Instance.Y * 16128) + (top * Global.tileLenght); }
			set
			{
				var part = MapWorker.Instance.Y * 16128;
				var partmax = MapWorker.Instance.Y * 16128 + 16128;

				if (value < part || value > partmax)
				{
					Top = 0;
					return;
				}

				Top = (value - part) / Global.tileLenght;
				OnPropertyChanged("mRight");
			}
		}

		[DisplayName("X")]
		[Category("Coordonnate 2")]
		[PropertyGridBrowsable(true)]
		public int mRight
		{
			get { return (MapWorker.Instance.X * 16128) + (right * Global.tileLenght); }
			set
			{
				var part = MapWorker.Instance.X * 16128;
				var partmax = MapWorker.Instance.X * 16128 + 16128;

				if (value < part || value > partmax)
				{
					right = 0;
					return;
				}

				right = (value - part) / Global.tileLenght;
				OnPropertyChanged("mRight");
			}
		}

		[DisplayName("Y")]
		[Category("Coordonnate 2")]
		[PropertyGridBrowsable(true)]
		public int mBottom
		{
			get { return (MapWorker.Instance.Y * 16128) + (bottom * Global.tileLenght); }
			set
			{
				var part = MapWorker.Instance.Y * 16128;
				var partmax = MapWorker.Instance.Y * 16128 + 16128;

				if (value < part || value > partmax)
				{
					bottom = 0;
					return;
				}

				bottom = (value - part) / Global.tileLenght;
				OnPropertyChanged("mBottom");
			}
		}

		[DisplayName("Description")]
        [PropertyGridBrowsable(true)]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [DisplayName("Scripts")]
        [PropertyGridBrowsable(true)]
        [Editor(typeof(CollectionEditorExtends), typeof(UITypeEditor))]
        public List<Function> Scripts
        {
            get { return script; }
            set
            {
                script = value;
                OnPropertyChanged("Script");
            }
        }

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Convert to pixel location
		/// </summary>
		/// <returns></returns>
		public PointF[] ToPoints()
		{
			var points = new List<PointF>();
			points.Add(new PointF(Left, Top));
			points.Add(new PointF(Right, Bottom));
			return _2DUtils.UnAdjustPolygonPoint(points, 1f, false, true);
		}

		public override string ToString()
		{
			return string.Format("Region : {0}", Description);
		}

		protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PropScriptInfo : INotifyPropertyChanged
    {
        private int propId = 0;
        private float x = 0;
        private float y = 0;
        private short modelId = 0;
        private List<Function> records = new List<Function>();

        [DisplayName("PropId")]
        [PropertyGridBrowsable(true)]
        public int PropId
        {
            get { return propId; }
            set
            {
                propId = value;
                OnPropertyChanged("PropId");
            }
        }
		
        [PropertyGridBrowsable(false)]
        public float X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged("X");
            }
        }
		
        [PropertyGridBrowsable(false)]
        public float Y
        {
            get { return y; }
            set
            {
                y = value;
                OnPropertyChanged("Y");
            }
		}

		[DisplayName("X")]
		[PropertyGridBrowsable(true)]
		public float mX
		{
			get { return (MapWorker.Instance.X * 16128) + x; }
			set
			{
				var part = MapWorker.Instance.X * 16128;
				var partmax = MapWorker.Instance.X * 16128 + 16128;

				if (value < part || value > partmax)
				{
					x = 0;
					return;
				}

				x = (value - part);
				OnPropertyChanged("X");
			}
		}

		[DisplayName("Y")]
		[PropertyGridBrowsable(true)]
		public float mY
		{
			get { return (MapWorker.Instance.Y * 16128) + y; }
			set
			{
				var part = MapWorker.Instance.Y * 16128;
				var partmax = MapWorker.Instance.Y * 16128 + 16128;

				if (value < part || value > partmax)
				{
					y = 0;
					return;
				}

				y = (value - part);
				OnPropertyChanged("Y");
			}
		}

		[DisplayName("Model Id")]
        [PropertyGridBrowsable(true)]
        public short ModelId
        {
            get { return modelId; }
            set
            {
                modelId = value;
                OnPropertyChanged("ModelId");
            }
        }

        [DisplayName("Scripts")]
        [PropertyGridBrowsable(true)]
        [Editor(typeof(CollectionEditorExtends), typeof(UITypeEditor))]
        public List<Function> Scripts
        {
            get { return records; }
            set
            {
                records = value;
                OnPropertyChanged("Records");
            }
        }

        /// <summary>
        /// Convert to pixel location
        /// </summary>
        /// <returns></returns>
        public PointF ToPoint()
        {
			return _2DUtils.UnAdjustPoint(new PointF(X, Y), 1f, false, false);
		}

		public override string ToString()
		{
			return string.Format("NPC : {0}", PropId);
		}

		protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Function : INotifyPropertyChanged
    {
        public string functionString = "";

        [DisplayName("Function string")]
        [PropertyGridBrowsable(true)]
        public string FunctionString
        {
            get { return functionString; }
            set
            {
                functionString = value;
                OnPropertyChanged("FunctionString");
            }
        }
        
        public override string ToString()
        {
            return string.Format("Function : {0}", FunctionString);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
