using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils.ExtensibleStorageUtils.SchemaWrapperTools
{
    /// <summary>
    /// A class to store schema field information
    /// </summary>
    [Serializable]
    public class FieldData
    {

        #region Constructors

        /// <summary>
        /// For serialization only -- Do not use.
        /// </summary>
        internal FieldData() { }

#if (REVIT2016 || REVIT2017 || REVIT2018 || REVIT2019 || REVIT2020 || REVIT2021)
        /// <summary>
        /// Create a new FieldData object
        /// </summary>
        /// <param name="name">The name of the field</param>
        /// <param name="typeIn">The AssemblyQualifiedName of the Field's data type</param>
        /// <param name="unit">The unit type of the Field (set to UT_Undefined for non-floating point types</param>
        public FieldData(string name, string typeIn, UnitType unit) : this(name, typeIn, unit, null)
        {
        }

        /// <summary>
        /// Create a new FieldData object
        /// </summary>
        /// <param name="name">The name of the field</param>
        /// <param name="typeIn">The AssemblyQualifiedName of the Field's data type</param>
        /// <param name="unit">The unit type of the Field (set to UT_Undefined for non-floating point types</param>
        /// <param name="subSchema">The SchemaWrapper of the field's subSchema, if the field is of type "Entity"</param>
        public FieldData(string name, string typeIn, UnitType unit, SchemaWrapper subSchema)
        {
            _mName = name;
            _mType = typeIn;
            _mUnit = unit;
            _mSubSchema = subSchema;
        }
#else
        /// <summary>
        /// Create a new FieldData object
        /// </summary>
        /// <param name="name">The name of the field</param>
        /// <param name="typeIn">The AssemblyQualifiedName of the Field's data type</param>
        /// <param name="unit">The unit type of the Field (set to UT_Undefined for non-floating point types</param>
        public FieldData(string name, string typeIn, ForgeTypeId unit) : this(name, typeIn, unit, null)
        {
        }

        /// <summary>
        /// Create a new FieldData object
        /// </summary>
        /// <param name="name">The name of the field</param>
        /// <param name="typeIn">The AssemblyQualifiedName of the Field's data type</param>
        /// <param name="unit">The unit type of the Field (set to UT_Undefined for non-floating point types</param>
        /// <param name="subSchema">The SchemaWrapper of the field's subSchema, if the field is of type "Entity"</param>
        public FieldData(string name, string typeIn, ForgeTypeId unit, SchemaWrapper subSchema)
        {
            m_Name = name;
            m_Type = typeIn;
            m_Unit = unit;
            m_SubSchema = subSchema;
        }
#endif

        #endregion

        #region Other helper functions
        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("   Field: ");
            strBuilder.Append(Name);
            strBuilder.Append(", ");
            strBuilder.Append(Type);
            strBuilder.Append(", ");
            strBuilder.Append(Unit.ToString());


            if (SubSchema != null)
            {
                strBuilder.Append(Environment.NewLine + "   " + SubSchema.ToString());
            }
            return strBuilder.ToString();
        }
        #endregion

        #region Properties
        /// <summary>
        /// The name of a schema field
        /// </summary>
        public string Name
        {
            get { return _mName; }
            set { _mName = value; }
        }

        /// <summary>
        /// The string representation of a schema field type (e.g. System.Int32)
        /// </summary>
        public string Type
        {
            get { return _mType; }
            set { _mType = value; }
        }

#if (REVIT2016 || REVIT2017 || REVIT2018 || REVIT2019 || REVIT2020 || REVIT2021)
        /// <summary>
        /// The Unit type of the field
        /// </summary>
        public UnitType Unit
        {
            get { return _mUnit; }
            set { _mUnit = value; }
        }
#else
        public ForgeTypeId Unit
        {
            get { return m_Unit; }
            set { m_Unit = value; }
        }
#endif
        /// <summary>
        /// The SchemaWrapper of the field's sub-Schema, if is of type "Entity"
        /// </summary>
        public SchemaWrapper SubSchema
        {
            get { return _mSubSchema; }
            set { _mSubSchema = value; }
        }
        #endregion

        #region Data
        private SchemaWrapper _mSubSchema;
        private string _mName;
        private string _mType;
#if (REVIT2016 || REVIT2017 || REVIT2018 || REVIT2019 || REVIT2020 || REVIT2021)
        private UnitType _mUnit;
#else
        private ForgeTypeId m_Unit;
#endif
        #endregion

    }
}
