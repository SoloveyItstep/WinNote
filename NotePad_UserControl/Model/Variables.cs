using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NotePad_UserControl.Model
{
    public class Variables
    {
        private static Variables variables { get; set; }

        private Variables()
        {
            categoriesList = new List<INotepadCategories>();
            textDocumentsList = new List<INotepadTextDocuments>();
        }

        public static Variables GetInstance
        {
            get
            {
                if (variables == null)
                    variables = new Variables();
                return variables;
            }
            set
            {
                variables = value;
            }
        }

        public List<INotepadCategories> categoriesList { get; set; }
        public List<INotepadTextDocuments> textDocumentsList { get; set; }
        public String currentCategoryName { get; set; }
        public Int32 currentCategoryId { get; set; }
    }

    public class CategoriesVariables
    {
        private static CategoriesVariables categories { get; set; }

        private CategoriesVariables()
        {
            borderList = new List<Border>();
            gridList = new List<Grid>();
            labelDictionary = new Dictionary<String,Label>();
        }

        public static CategoriesVariables GetInstance
        {
            get
            {
                if (categories == null)
                    categories = new CategoriesVariables();
                return categories;
            }
            set
            {
                categories = value;
            }
        }

        public List<Border> borderList { get; set; }
        public List<Grid> gridList { get; set; }
        public Dictionary<String,Label> labelDictionary { get; set; }
    }

    public class TextVariables
    {
        private static TextVariables texts { get; set; }

        private TextVariables()
        {
            borderList = new List<Border>();
            gridList = new List<Grid>();
            labelDictionary = new Dictionary<Int32,Label>();
        }

        public static TextVariables GetInstance
        {
            get
            {
                if (texts == null)
                    texts = new TextVariables();
                return texts;
            }
            set
            {
                texts = value;
            }
        }

        public List<Border> borderList { get; set; }
        public List<Grid> gridList { get; set; }
        public Dictionary<Int32, Label> labelDictionary { get; set; }
    }
}
