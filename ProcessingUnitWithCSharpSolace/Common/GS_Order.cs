using System;
using System.Text;
using GigaSpaces.Core.Metadata;

namespace Piper.Common
{
    /// <summary>
    /// Represnts a Order object
    /// </summary>	
    [SpaceClass(Persist = true)]
    public class GS_Order : IType
    {
        //		private long _OPID = -1L;
        private long _OrderID = -1L;
        private String _Symbol;
        private long _Quantity = -1L;
        private Nullable<Double> _Price;
        private long _CalCumQty = -1L;
        private Nullable<Double> _CalExecValue;

        // Partion ID
        //	[SpaceRouting]
        /*		public long OPID
                {
                    get { return _OPID; }
                    set { _OPID = value; }
                }*/

        //[SpaceProperty(AliasName = "OrderID")]
        [SpaceID]
        [SpaceRouting]
        [SpaceProperty(NullValue = -1)]
        public long OrderID
        {
            get { return _OrderID; }
            set { _OrderID = value; }
        }

        private long _TraderID = -1L;
        [SpaceProperty(AliasName = "TraderID", NullValue = -1)]
        public long TraderID
        {
            get { return this._TraderID; }
            set { this._TraderID = value; }
        }


        [SpaceProperty(AliasName = "Symbol")]
        public String Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }

        [SpaceProperty(AliasName = "Quantity", NullValue = -1)]
        public long Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; }
        }

        private long _CumQty = -1L;
        [SpaceProperty(AliasName = "CumQty", NullValue = -1)]
        public long CumQty
        {
            get { return this._CumQty; }
            set { this._CumQty = value; }
        }


        [SpaceProperty(AliasName = "Price")]
        public Nullable<Double> Price
        {
            get { return _Price; }
            set { _Price = value; }
        }

        [SpaceProperty(AliasName = "CalCumQty", NullValue = -1)]
        public long CalCumQty
        {
            get { return _CalCumQty; }
            set { _CalCumQty = value; }
        }
        [SpaceProperty(AliasName = "CalExecValue")]
        public Nullable<Double> CalExecValue
        {
            get { return _CalExecValue; }
            set { _CalExecValue = value; }
        }

        private long _FldInt_1 = -1L;
        [SpaceProperty(AliasName = "FldInt_1", NullValue = -1)]
        public long FldInt_1
        {
            get { return this._FldInt_1; }
            set { this._FldInt_1 = value; }
        }

        private long _FldInt_2 = -1L;
        [SpaceProperty(AliasName = "FldInt_2", NullValue = -1)]
        public long FldInt_2
        {
            get { return this._FldInt_2; }
            set { this._FldInt_2 = value; }
        }

        private long _FldInt_3 = -1L;
        [SpaceProperty(AliasName = "FldInt_3", NullValue = -1)]
        public long FldInt_3
        {
            get { return this._FldInt_3; }
            set { this._FldInt_3 = value; }
        }

        private long _FldInt_4 = -1L;
        [SpaceProperty(AliasName = "FldInt_4", NullValue = -1)]
        public long FldInt_4
        {
            get { return this._FldInt_4; }
            set { this._FldInt_4 = value; }
        }

        private long _FldInt_5 = -1L;
        [SpaceProperty(AliasName = "FldInt_5", NullValue = -1)]
        public long FldInt_5
        {
            get { return this._FldInt_5; }
            set { this._FldInt_5 = value; }
        }

        private long _FldInt_6 = -1L;
        [SpaceProperty(AliasName = "FldInt_6", NullValue = -1)]
        public long FldInt_6
        {
            get { return this._FldInt_6; }
            set { this._FldInt_6 = value; }
        }

        private long _FldInt_7 = -1L;
        [SpaceProperty(AliasName = "FldInt_7", NullValue = -1)]
        public long FldInt_7
        {
            get { return this._FldInt_7; }
            set { this._FldInt_7 = value; }
        }

        private long _FldInt_8 = -1L;
        [SpaceProperty(AliasName = "FldInt_8", NullValue = -1)]
        public long FldInt_8
        {
            get { return this._FldInt_8; }
            set { this._FldInt_8 = value; }
        }

        private long _FldInt_9 = -1L;
        [SpaceProperty(AliasName = "FldInt_9", NullValue = -1)]
        public long FldInt_9
        {
            get { return this._FldInt_9; }
            set { this._FldInt_9 = value; }
        }

        private long _FldInt_10 = -1L;
        [SpaceProperty(AliasName = "FldInt_10", NullValue = -1)]
        public long FldInt_10
        {
            get { return this._FldInt_10; }
            set { this._FldInt_10 = value; }
        }

        private long _FldInt_11 = -1L;
        [SpaceProperty(AliasName = "FldInt_11", NullValue = -1)]
        public long FldInt_11
        {
            get { return this._FldInt_11; }
            set { this._FldInt_11 = value; }
        }

        private long _FldInt_12 = -1L;
        [SpaceProperty(AliasName = "FldInt_12", NullValue = -1)]
        public long FldInt_12
        {
            get { return this._FldInt_12; }
            set { this._FldInt_12 = value; }
        }

        private long _FldInt_13 = -1L;
        [SpaceProperty(AliasName = "FldInt_13", NullValue = -1)]
        public long FldInt_13
        {
            get { return this._FldInt_13; }
            set { this._FldInt_13 = value; }
        }

        private long _FldInt_14 = -1L;
        [SpaceProperty(AliasName = "FldInt_14", NullValue = -1)]
        public long FldInt_14
        {
            get { return this._FldInt_14; }
            set { this._FldInt_14 = value; }
        }

        private long _FldInt_15 = -1L;
        [SpaceProperty(AliasName = "FldInt_15", NullValue = -1)]
        public long FldInt_15
        {
            get { return this._FldInt_15; }
            set { this._FldInt_15 = value; }
        }

        private long _FldInt_16 = -1L;
        [SpaceProperty(AliasName = "FldInt_16", NullValue = -1)]
        public long FldInt_16
        {
            get { return this._FldInt_16; }
            set { this._FldInt_16 = value; }
        }

        private long _FldInt_17 = -1L;
        [SpaceProperty(AliasName = "FldInt_17", NullValue = -1)]
        public long FldInt_17
        {
            get { return this._FldInt_17; }
            set { this._FldInt_17 = value; }
        }

        private long _FldInt_18 = -1L;
        [SpaceProperty(AliasName = "FldInt_18", NullValue = -1)]
        public long FldInt_18
        {
            get { return this._FldInt_18; }
            set { this._FldInt_18 = value; }
        }

        private long _FldInt_19 = -1L;
        [SpaceProperty(AliasName = "FldInt_19", NullValue = -1)]
        public long FldInt_19
        {
            get { return this._FldInt_19; }
            set { this._FldInt_19 = value; }
        }

        private long _FldInt_20 = -1L;
        [SpaceProperty(AliasName = "FldInt_20", NullValue = -1)]
        public long FldInt_20
        {
            get { return this._FldInt_20; }
            set { this._FldInt_20 = value; }
        }

        private long _FldInt_21 = -1L;
        [SpaceProperty(AliasName = "FldInt_21", NullValue = -1)]
        public long FldInt_21
        {
            get { return this._FldInt_21; }
            set { this._FldInt_21 = value; }
        }

        private long _FldInt_22 = -1L;
        [SpaceProperty(AliasName = "FldInt_22", NullValue = -1)]
        public long FldInt_22
        {
            get { return this._FldInt_22; }
            set { this._FldInt_22 = value; }
        }

        private long _FldInt_23 = -1L;
        [SpaceProperty(AliasName = "FldInt_23", NullValue = -1)]
        public long FldInt_23
        {
            get { return this._FldInt_23; }
            set { this._FldInt_23 = value; }
        }

        private long _FldInt_24 = -1L;
        [SpaceProperty(AliasName = "FldInt_24", NullValue = -1)]
        public long FldInt_24
        {
            get { return this._FldInt_24; }
            set { this._FldInt_24 = value; }
        }

        private long _FldInt_25 = -1L;
        [SpaceProperty(AliasName = "FldInt_25", NullValue = -1)]
        public long FldInt_25
        {
            get { return this._FldInt_25; }
            set { this._FldInt_25 = value; }
        }

        private long _FldInt_26 = -1L;
        [SpaceProperty(AliasName = "FldInt_26", NullValue = -1)]
        public long FldInt_26
        {
            get { return this._FldInt_26; }
            set { this._FldInt_26 = value; }
        }

        private long _FldInt_27 = -1L;
        [SpaceProperty(AliasName = "FldInt_27", NullValue = -1)]
        public long FldInt_27
        {
            get { return this._FldInt_27; }
            set { this._FldInt_27 = value; }
        }

        private long _FldInt_28 = -1L;
        [SpaceProperty(AliasName = "FldInt_28", NullValue = -1)]
        public long FldInt_28
        {
            get { return this._FldInt_28; }
            set { this._FldInt_28 = value; }
        }

        private long _FldInt_29 = -1L;
        [SpaceProperty(AliasName = "FldInt_29", NullValue = -1)]
        public long FldInt_29
        {
            get { return this._FldInt_29; }
            set { this._FldInt_29 = value; }
        }

        private long _FldInt_30 = -1L;
        [SpaceProperty(AliasName = "FldInt_30", NullValue = -1)]
        public long FldInt_30
        {
            get { return this._FldInt_30; }
            set { this._FldInt_30 = value; }
        }

        private long _FldInt_31 = -1L;
        [SpaceProperty(AliasName = "FldInt_31", NullValue = -1)]
        public long FldInt_31
        {
            get { return this._FldInt_31; }
            set { this._FldInt_31 = value; }
        }

        private long _FldInt_32 = -1L;
        [SpaceProperty(AliasName = "FldInt_32", NullValue = -1)]
        public long FldInt_32
        {
            get { return this._FldInt_32; }
            set { this._FldInt_32 = value; }
        }

        private long _FldInt_33 = -1L;
        [SpaceProperty(AliasName = "FldInt_33", NullValue = -1)]
        public long FldInt_33
        {
            get { return this._FldInt_33; }
            set { this._FldInt_33 = value; }
        }

        private long _FldInt_34 = -1L;
        [SpaceProperty(AliasName = "FldInt_34", NullValue = -1)]
        public long FldInt_34
        {
            get { return this._FldInt_34; }
            set { this._FldInt_34 = value; }
        }

        private long _FldInt_35 = -1L;
        [SpaceProperty(AliasName = "FldInt_35", NullValue = -1)]
        public long FldInt_35
        {
            get { return this._FldInt_35; }
            set { this._FldInt_35 = value; }
        }

        private long _FldInt_36 = -1L;
        [SpaceProperty(AliasName = "FldInt_36", NullValue = -1)]
        public long FldInt_36
        {
            get { return this._FldInt_36; }
            set { this._FldInt_36 = value; }
        }

        private long _FldInt_37 = -1L;
        [SpaceProperty(AliasName = "FldInt_37", NullValue = -1)]
        public long FldInt_37
        {
            get { return this._FldInt_37; }
            set { this._FldInt_37 = value; }
        }

        private long _FldInt_38 = -1L;
        [SpaceProperty(AliasName = "FldInt_38", NullValue = -1)]
        public long FldInt_38
        {
            get { return this._FldInt_38; }
            set { this._FldInt_38 = value; }
        }

        private long _FldInt_39 = -1L;
        [SpaceProperty(AliasName = "FldInt_39", NullValue = -1)]
        public long FldInt_39
        {
            get { return this._FldInt_39; }
            set { this._FldInt_39 = value; }
        }

        private long _FldInt_40 = -1L;
        [SpaceProperty(AliasName = "FldInt_40", NullValue = -1)]
        public long FldInt_40
        {
            get { return this._FldInt_40; }
            set { this._FldInt_40 = value; }
        }

        private DateTime? _FldTime_1;
        [SpaceProperty(AliasName = "FldTime_1")]
        public DateTime? FldTime_1
        {
            get { return this._FldTime_1; }
            set { this._FldTime_1 = value; }
        }

        private DateTime? _FldTime_2;
        [SpaceProperty(AliasName = "FldTime_2")]
        public DateTime? FldTime_2
        {
            get { return this._FldTime_2; }
            set { this._FldTime_2 = value; }
        }

        private DateTime? _FldTime_3;
        [SpaceProperty(AliasName = "FldTime_3")]
        public DateTime? FldTime_3
        {
            get { return this._FldTime_3; }
            set { this._FldTime_3 = value; }
        }

        private DateTime? _FldTime_4;
        [SpaceProperty(AliasName = "FldTime_4")]
        public DateTime? FldTime_4
        {
            get { return this._FldTime_4; }
            set { this._FldTime_4 = value; }
        }

        private DateTime? _FldTime_5;
        [SpaceProperty(AliasName = "FldTime_5")]
        public DateTime? FldTime_5
        {
            get { return this._FldTime_5; }
            set { this._FldTime_5 = value; }
        }

        private DateTime? _FldTime_6;
        [SpaceProperty(AliasName = "FldTime_6")]
        public DateTime? FldTime_6
        {
            get { return this._FldTime_6; }
            set { this._FldTime_6 = value; }
        }

        private DateTime? _FldTime_7;
        [SpaceProperty(AliasName = "FldTime_7")]
        public DateTime? FldTime_7
        {
            get { return this._FldTime_7; }
            set { this._FldTime_7 = value; }
        }

        private DateTime? _FldTime_8;
        [SpaceProperty(AliasName = "FldTime_8")]
        public DateTime? FldTime_8
        {
            get { return this._FldTime_8; }
            set { this._FldTime_8 = value; }
        }

        private string _FldStr_1;
        [SpaceProperty(AliasName = "FldStr_1")]
        public String FldStr_1
        {
            get { return this._FldStr_1; }
            set { this._FldStr_1 = value; }
        }

        private string _FldStr_2;
        [SpaceProperty(AliasName = "FldStr_2")]
        public String FldStr_2
        {
            get { return this._FldStr_2; }
            set { this._FldStr_2 = value; }
        }

        private string _FldStr_3;
        [SpaceProperty(AliasName = "FldStr_3")]
        public String FldStr_3
        {
            get { return this._FldStr_3; }
            set { this._FldStr_3 = value; }
        }

        private string _FldStr_4;
        [SpaceProperty(AliasName = "FldStr_4")]
        public String FldStr_4
        {
            get { return this._FldStr_4; }
            set { this._FldStr_4 = value; }
        }

        private string _FldStr_5;
        [SpaceProperty(AliasName = "FldStr_5")]
        public String FldStr_5
        {
            get { return this._FldStr_5; }
            set { this._FldStr_5 = value; }
        }

        private string _FldStr_6;
        [SpaceProperty(AliasName = "FldStr_6")]
        public String FldStr_6
        {
            get { return this._FldStr_6; }
            set { this._FldStr_6 = value; }
        }

        private string _FldStr_7;
        [SpaceProperty(AliasName = "FldStr_7")]
        public String FldStr_7
        {
            get { return this._FldStr_7; }
            set { this._FldStr_7 = value; }
        }

        private string _FldStr_8;
        [SpaceProperty(AliasName = "FldStr_8")]
        public String FldStr_8
        {
            get { return this._FldStr_8; }
            set { this._FldStr_8 = value; }
        }

        private string _FldStr_9;
        [SpaceProperty(AliasName = "FldStr_9")]
        public String FldStr_9
        {
            get { return this._FldStr_9; }
            set { this._FldStr_9 = value; }
        }

        private string _FldStr_10;
        [SpaceProperty(AliasName = "FldStr_10")]
        public String FldStr_10
        {
            get { return this._FldStr_10; }
            set { this._FldStr_10 = value; }
        }

        private string _FldStr_11;
        [SpaceProperty(AliasName = "FldStr_11")]
        public String FldStr_11
        {
            get { return this._FldStr_11; }
            set { this._FldStr_11 = value; }
        }

        private string _FldStr_12;
        [SpaceProperty(AliasName = "FldStr_12")]
        public String FldStr_12
        {
            get { return this._FldStr_12; }
            set { this._FldStr_12 = value; }
        }

        private string _FldStr_13;
        [SpaceProperty(AliasName = "FldStr_13")]
        public String FldStr_13
        {
            get { return this._FldStr_13; }
            set { this._FldStr_13 = value; }
        }

        private string _FldStr_14;
        [SpaceProperty(AliasName = "FldStr_14")]
        public String FldStr_14
        {
            get { return this._FldStr_14; }
            set { this._FldStr_14 = value; }
        }

        private string _FldStr_15;
        [SpaceProperty(AliasName = "FldStr_15")]
        public String FldStr_15
        {
            get { return this._FldStr_15; }
            set { this._FldStr_15 = value; }
        }

        private string _FldStr_16;
        [SpaceProperty(AliasName = "FldStr_16")]
        public String FldStr_16
        {
            get { return this._FldStr_16; }
            set { this._FldStr_16 = value; }
        }

        private string _FldStr_17;
        [SpaceProperty(AliasName = "FldStr_17")]
        public String FldStr_17
        {
            get { return this._FldStr_17; }
            set { this._FldStr_17 = value; }
        }

        private string _FldStr_18;
        [SpaceProperty(AliasName = "FldStr_18")]
        public String FldStr_18
        {
            get { return this._FldStr_18; }
            set { this._FldStr_18 = value; }
        }

        private string _FldStr_19;
        [SpaceProperty(AliasName = "FldStr_19")]
        public String FldStr_19
        {
            get { return this._FldStr_19; }
            set { this._FldStr_19 = value; }
        }

        private string _FldStr_20;
        [SpaceProperty(AliasName = "FldStr_20")]
        public String FldStr_20
        {
            get { return this._FldStr_20; }
            set { this._FldStr_20 = value; }
        }

        private string _FldStr_21;
        [SpaceProperty(AliasName = "FldStr_21")]
        public String FldStr_21
        {
            get { return this._FldStr_21; }
            set { this._FldStr_21 = value; }
        }

        private string _FldStr_22;
        [SpaceProperty(AliasName = "FldStr_22")]
        public String FldStr_22
        {
            get { return this._FldStr_22; }
            set { this._FldStr_22 = value; }
        }

        private string _FldStr_23;
        [SpaceProperty(AliasName = "FldStr_23")]
        public String FldStr_23
        {
            get { return this._FldStr_23; }
            set { this._FldStr_23 = value; }
        }

        private string _FldStr_24;
        [SpaceProperty(AliasName = "FldStr_24")]
        public String FldStr_24
        {
            get { return this._FldStr_24; }
            set { this._FldStr_24 = value; }
        }

        private string _FldStr_25;
        [SpaceProperty(AliasName = "FldStr_25")]
        public String FldStr_25
        {
            get { return this._FldStr_25; }
            set { this._FldStr_25 = value; }
        }

        private string _FldStr_26;
        [SpaceProperty(AliasName = "FldStr_26")]
        public String FldStr_26
        {
            get { return this._FldStr_26; }
            set { this._FldStr_26 = value; }
        }

        private string _FldStr_27;
        [SpaceProperty(AliasName = "FldStr_27")]
        public String FldStr_27
        {
            get { return this._FldStr_27; }
            set { this._FldStr_27 = value; }
        }

        private string _FldStr_28;
        [SpaceProperty(AliasName = "FldStr_28")]
        public String FldStr_28
        {
            get { return this._FldStr_28; }
            set { this._FldStr_28 = value; }
        }

        private string _FldStr_29;
        [SpaceProperty(AliasName = "FldStr_29")]
        public String FldStr_29
        {
            get { return this._FldStr_29; }
            set { this._FldStr_29 = value; }
        }

        private string _FldStr_30;
        [SpaceProperty(AliasName = "FldStr_30")]
        public String FldStr_30
        {
            get { return this._FldStr_30; }
            set { this._FldStr_30 = value; }
        }

        private string _FldStr_31;
        [SpaceProperty(AliasName = "FldStr_31")]
        public String FldStr_31
        {
            get { return this._FldStr_31; }
            set { this._FldStr_31 = value; }
        }

        private string _FldStr_32;
        [SpaceProperty(AliasName = "FldStr_32")]
        public String FldStr_32
        {
            get { return this._FldStr_32; }
            set { this._FldStr_32 = value; }
        }

        private string _FldStr_33;
        [SpaceProperty(AliasName = "FldStr_33")]
        public String FldStr_33
        {
            get { return this._FldStr_33; }
            set { this._FldStr_33 = value; }
        }

        private string _FldStr_34;
        [SpaceProperty(AliasName = "FldStr_34")]
        public String FldStr_34
        {
            get { return this._FldStr_34; }
            set { this._FldStr_34 = value; }
        }

        private string _FldStr_35;
        [SpaceProperty(AliasName = "FldStr_35")]
        public String FldStr_35
        {
            get { return this._FldStr_35; }
            set { this._FldStr_35 = value; }
        }

        private string _FldStr_36;
        [SpaceProperty(AliasName = "FldStr_36")]
        public String FldStr_36
        {
            get { return this._FldStr_36; }
            set { this._FldStr_36 = value; }
        }

        private string _FldStr_37;
        [SpaceProperty(AliasName = "FldStr_37")]
        public String FldStr_37
        {
            get { return this._FldStr_37; }
            set { this._FldStr_37 = value; }
        }

        private string _FldStr_38;
        [SpaceProperty(AliasName = "FldStr_38")]
        public String FldStr_38
        {
            get { return this._FldStr_38; }
            set { this._FldStr_38 = value; }
        }

        private string _FldStr_39;
        [SpaceProperty(AliasName = "FldStr_39")]
        public String FldStr_39
        {
            get { return this._FldStr_39; }
            set { this._FldStr_39 = value; }
        }

        private string _FldStr_40;
        [SpaceProperty(AliasName = "FldStr_40")]
        public String FldStr_40
        {
            get { return this._FldStr_40; }
            set { this._FldStr_40 = value; }
        }

        private double _FldDbl_1 = -1;
        [SpaceProperty(AliasName = "FldDbl_1", NullValue = -1)]
        public Double FldDbl_1
        {
            get { return this._FldDbl_1; }
            set { this._FldDbl_1 = value; }
        }

        private double _FldDbl_2 = -1;
        [SpaceProperty(AliasName = "FldDbl_2", NullValue = -1)]
        public Double FldDbl_2
        {
            get { return this._FldDbl_2; }
            set { this._FldDbl_2 = value; }
        }

        private double _FldDbl_3 = -1;
        [SpaceProperty(AliasName = "FldDbl_3", NullValue = -1)]
        public Double FldDbl_3
        {
            get { return this._FldDbl_3; }
            set { this._FldDbl_3 = value; }
        }

        private double _FldDbl_4 = -1;
        [SpaceProperty(AliasName = "FldDbl_4", NullValue = -1)]
        public Double FldDbl_4
        {
            get { return this._FldDbl_4; }
            set { this._FldDbl_4 = value; }
        }

        private double _FldDbl_5 = -1;
        [SpaceProperty(AliasName = "FldDbl_5", NullValue = -1)]
        public Double FldDbl_5
        {
            get { return this._FldDbl_5; }
            set { this._FldDbl_5 = value; }
        }

        private double _FldDbl_6 = -1;
        [SpaceProperty(AliasName = "FldDbl_6", NullValue = -1)]
        public Double FldDbl_6
        {
            get { return this._FldDbl_6; }
            set { this._FldDbl_6 = value; }
        }

        private double _FldDbl_7 = -1;
        [SpaceProperty(AliasName = "FldDbl_7", NullValue = -1)]
        public Double FldDbl_7
        {
            get { return this._FldDbl_7; }
            set { this._FldDbl_7 = value; }
        }

        private double _FldDbl_8 = -1;
        [SpaceProperty(AliasName = "FldDbl_8", NullValue = -1)]
        public Double FldDbl_8
        {
            get { return this._FldDbl_8; }
            set { this._FldDbl_8 = value; }
        }

        private double _FldDbl_9 = -1;
        [SpaceProperty(AliasName = "FldDbl_9", NullValue = -1)]
        public Double FldDbl_9
        {
            get { return this._FldDbl_9; }
            set { this._FldDbl_9 = value; }
        }

        private double _FldDbl_10 = -1;
        [SpaceProperty(AliasName = "FldDbl_10", NullValue = -1)]
        public Double FldDbl_10
        {
            get { return this._FldDbl_10; }
            set { this._FldDbl_10 = value; }
        }

        private double _FldDbl_11 = -1;
        [SpaceProperty(AliasName = "FldDbl_11", NullValue = -1)]
        public Double FldDbl_11
        {
            get { return this._FldDbl_11; }
            set { this._FldDbl_11 = value; }
        }

        private double _FldDbl_12 = -1;
        [SpaceProperty(AliasName = "FldDbl_12", NullValue = -1)]
        public Double FldDbl_12
        {
            get { return this._FldDbl_12; }
            set { this._FldDbl_12 = value; }
        }

        private double _FldDbl_13 = -1;
        [SpaceProperty(AliasName = "FldDbl_13", NullValue = -1)]
        public Double FldDbl_13
        {
            get { return this._FldDbl_13; }
            set { this._FldDbl_13 = value; }
        }

        private double _FldDbl_14 = -1;
        [SpaceProperty(AliasName = "FldDbl_14", NullValue = -1)]
        public Double FldDbl_14
        {
            get { return this._FldDbl_14; }
            set { this._FldDbl_14 = value; }
        }

        private double _FldDbl_15 = -1;
        [SpaceProperty(AliasName = "FldDbl_15", NullValue = -1)]
        public Double FldDbl_15
        {
            get { return this._FldDbl_15; }
            set { this._FldDbl_15 = value; }
        }

        private double _FldDbl_16 = -1;
        [SpaceProperty(AliasName = "FldDbl_16", NullValue = -1)]
        public Double FldDbl_16
        {
            get { return this._FldDbl_16; }
            set { this._FldDbl_16 = value; }
        }

        private double _FldDbl_17 = -1;
        [SpaceProperty(AliasName = "FldDbl_17", NullValue = -1)]
        public Double FldDbl_17
        {
            get { return this._FldDbl_17; }
            set { this._FldDbl_17 = value; }
        }

        private double _FldDbl_18 = -1;
        [SpaceProperty(AliasName = "FldDbl_18", NullValue = -1)]
        public Double FldDbl_18
        {
            get { return this._FldDbl_18; }
            set { this._FldDbl_18 = value; }
        }

        private double _FldDbl_19 = -1;
        [SpaceProperty(AliasName = "FldDbl_19", NullValue = -1)]
        public Double FldDbl_19
        {
            get { return this._FldDbl_19; }
            set { this._FldDbl_19 = value; }
        }

        private double _FldDbl_20 = -1;
        [SpaceProperty(AliasName = "FldDbl_20", NullValue = -1)]
        public Double FldDbl_20
        {
            get { return this._FldDbl_20; }
            set { this._FldDbl_20 = value; }
        }

        private double _FldDbl_21 = -1;
        [SpaceProperty(AliasName = "FldDbl_21", NullValue = -1)]
        public Double FldDbl_21
        {
            get { return this._FldDbl_21; }
            set { this._FldDbl_21 = value; }
        }

        private double _FldDbl_22 = -1;
        [SpaceProperty(AliasName = "FldDbl_22", NullValue = -1)]
        public Double FldDbl_22
        {
            get { return this._FldDbl_22; }
            set { this._FldDbl_22 = value; }
        }

        private double _FldDbl_23 = -1;
        [SpaceProperty(AliasName = "FldDbl_23", NullValue = -1)]
        public Double FldDbl_23
        {
            get { return this._FldDbl_23; }
            set { this._FldDbl_23 = value; }
        }

        private double _FldDbl_24 = -1;
        [SpaceProperty(AliasName = "FldDbl_24", NullValue = -1)]
        public Double FldDbl_24
        {
            get { return this._FldDbl_24; }
            set { this._FldDbl_24 = value; }
        }

        private double _FldDbl_25 = -1;
        [SpaceProperty(AliasName = "FldDbl_25", NullValue = -1)]
        public Double FldDbl_25
        {
            get { return this._FldDbl_25; }
            set { this._FldDbl_25 = value; }
        }

        private double _FldDbl_26 = -1;
        [SpaceProperty(AliasName = "FldDbl_26", NullValue = -1)]
        public Double FldDbl_26
        {
            get { return this._FldDbl_26; }
            set { this._FldDbl_26 = value; }
        }

        private double _FldDbl_27 = -1;
        [SpaceProperty(AliasName = "FldDbl_27", NullValue = -1)]
        public Double FldDbl_27
        {
            get { return this._FldDbl_27; }
            set { this._FldDbl_27 = value; }
        }

        private double _FldDbl_28 = -1;
        [SpaceProperty(AliasName = "FldDbl_28", NullValue = -1)]
        public Double FldDbl_28
        {
            get { return this._FldDbl_28; }
            set { this._FldDbl_28 = value; }
        }

        private double _FldDbl_29 = -1;
        [SpaceProperty(AliasName = "FldDbl_29", NullValue = -1)]
        public Double FldDbl_29
        {
            get { return this._FldDbl_29; }
            set { this._FldDbl_29 = value; }
        }

        private double _FldDbl_30 = -1;
        [SpaceProperty(AliasName = "FldDbl_30", NullValue = -1)]
        public Double FldDbl_30
        {
            get { return this._FldDbl_30; }
            set { this._FldDbl_30 = value; }
        }

        private double _FldDbl_31 = -1;
        [SpaceProperty(AliasName = "FldDbl_31", NullValue = -1)]
        public Double FldDbl_31
        {
            get { return this._FldDbl_31; }
            set { this._FldDbl_31 = value; }
        }

        private double _FldDbl_32 = -1;
        [SpaceProperty(AliasName = "FldDbl_32", NullValue = -1)]
        public Double FldDbl_32
        {
            get { return this._FldDbl_32; }
            set { this._FldDbl_32 = value; }
        }

        private double _FldDbl_33 = -1;
        [SpaceProperty(AliasName = "FldDbl_33", NullValue = -1)]
        public Double FldDbl_33
        {
            get { return this._FldDbl_33; }
            set { this._FldDbl_33 = value; }
        }

        private double _FldDbl_34 = -1;
        [SpaceProperty(AliasName = "FldDbl_34", NullValue = -1)]
        public Double FldDbl_34
        {
            get { return this._FldDbl_34; }
            set { this._FldDbl_34 = value; }
        }

        private double _FldDbl_35 = -1;
        [SpaceProperty(AliasName = "FldDbl_35", NullValue = -1)]
        public Double FldDbl_35
        {
            get { return this._FldDbl_35; }
            set { this._FldDbl_35 = value; }
        }

        private double _FldDbl_36 = -1;
        [SpaceProperty(AliasName = "FldDbl_36", NullValue = -1)]
        public Double FldDbl_36
        {
            get { return this._FldDbl_36; }
            set { this._FldDbl_36 = value; }
        }

        private double _FldDbl_37 = -1;
        [SpaceProperty(AliasName = "FldDbl_37", NullValue = -1)]
        public Double FldDbl_37
        {
            get { return this._FldDbl_37; }
            set { this._FldDbl_37 = value; }
        }

        private double _FldDbl_38 = -1;
        [SpaceProperty(AliasName = "FldDbl_38", NullValue = -1)]
        public Double FldDbl_38
        {
            get { return this._FldDbl_38; }
            set { this._FldDbl_38 = value; }
        }

        private double _FldDbl_39 = -1;
        [SpaceProperty(AliasName = "FldDbl_39", NullValue = -1)]
        public Double FldDbl_39
        {
            get { return this._FldDbl_39; }
            set { this._FldDbl_39 = value; }
        }

        private double _FldDbl_40 = -1;
        [SpaceProperty(AliasName = "FldDbl_40", NullValue = -1)]
        public Double FldDbl_40
        {
            get { return this._FldDbl_40; }
            set { this._FldDbl_40 = value; }
        }

        public string serializeToJsonString()
        {
            GS_Order order = this;
            StringBuilder orderJsonStr = new StringBuilder();
            orderJsonStr.Append("\"type\":\"");
            orderJsonStr.Append("GS_Order");
            orderJsonStr.Append("\",\"spaceId\":\"");
            orderJsonStr.Append("OrderID\"");
            //            orderJsonStr.Append("_ID\"");
            orderJsonStr.Append(",\"payload\":[{");

            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("OrderID");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.OrderID);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("TraderID");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.TraderID);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("Symbol");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.Symbol);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("Quantity");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.Quantity);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("CumQty");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.CumQty);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("Price");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.Price);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("CalCumQty");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.CalCumQty);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("CalExecValue");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.CalExecValue);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_1");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_1);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_2");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_2);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_3");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_3);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_4");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_4);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_5");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_5);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_6");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_6);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_7");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_7);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_8");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_8);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_9");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_9);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_10");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_10);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_11");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_11);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_12");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_12);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_13");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_13);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_14");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_14);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_15");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_15);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_16");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_16);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_17");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_17);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_18");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_18);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_19");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_19);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_20");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_20);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_21");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_21);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_22");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_22);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_23");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_23);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_24");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_24);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_25");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_25);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_26");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_26);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_27");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_27);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_28");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_28);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_29");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_29);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_30");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_30);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_31");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_31);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_32");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_32);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_33");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_33);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_34");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_34);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_35");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_35);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_36");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_36);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_37");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_37);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_38");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_38);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_39");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_39);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldInt_40");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldInt_40);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldTime_1");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldTime_1);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldTime_2");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldTime_2);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldTime_3");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldTime_3);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldTime_4");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldTime_4);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldTime_5");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldTime_5);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldTime_6");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldTime_6);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldTime_7");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldTime_7);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldTime_8");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldTime_8);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_1");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_1);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_2");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_2);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_3");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_3);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_4");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_4);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_5");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_5);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_6");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_6);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_7");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_7);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_8");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_8);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_9");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_9);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_10");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_10);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_11");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_11);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_12");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_12);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_13");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_13);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_14");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_14);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_15");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_15);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_16");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_16);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_17");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_17);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_18");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_18);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_19");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_19);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_20");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_20);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_21");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_21);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_22");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_22);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_23");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_23);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_24");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_24);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_25");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_25);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_26");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_26);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_27");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_27);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_28");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_28);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_29");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_29);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_30");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_30);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_31");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_31);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_32");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_32);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_33");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_33);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_34");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_34);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_35");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_35);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_36");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_36);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_37");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_37);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_38");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_38);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_39");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_39);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldStr_40");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldStr_40);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_1");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_1);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_2");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_2);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_3");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_3);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_4");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_4);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_5");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_5);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_6");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_6);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_7");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_7);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_8");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_8);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_9");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_9);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_10");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_10);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_11");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_11);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_12");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_12);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_13");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_13);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_14");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_14);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_15");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_15);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_16");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_16);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_17");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_17);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_18");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_18);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_19");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_19);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_20");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_20);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_21");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_21);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_22");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_22);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_23");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_23);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_24");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_24);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_25");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_25);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_26");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_26);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_27");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_27);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_28");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_28);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_29");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_29);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_30");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_30);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_31");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_31);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_32");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_32);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_33");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_33);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_34");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_34);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_35");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_35);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_36");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_36);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_37");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_37);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_38");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_38);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_39");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_39);
            orderJsonStr.Append("\"},{");
            orderJsonStr.Append("\"columnName\":\"");
            orderJsonStr.Append("FldDbl_40");
            orderJsonStr.Append("\",\"value\":\"");
            orderJsonStr.Append(order.FldDbl_40);
            orderJsonStr.Append("\"}");
            return orderJsonStr.ToString();
        }

        public IType deSerializeFromJsonString(string json)
        {
            throw new NotImplementedException();
        }
    }
}
