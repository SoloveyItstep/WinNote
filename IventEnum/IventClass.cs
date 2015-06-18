using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectCase_M_;

namespace IventEnum
{
    public class IventClass
    {
        #region для оповищения о новой оплате.

        public static event EventHandler<PurseGetSize> GetSizePurse;

        public static void SetPurse(string str, double size)
        {
            if (GetSizePurse != null)
            {
                GetSizePurse(null, new PurseGetSize(str, size));
            }
        }

        #endregion

        #region передача названия верхней части листа всех записел пнктов.

        public static event EventHandler<InfoGetTopText> GetTopTextInfo;

        public static void SetTextInfo(string str)
        {
            if (GetTopTextInfo != null)
            {
                GetTopTextInfo(null, new InfoGetTopText(str));
            }
        }

        #endregion

        #region обновляет лист записей категории

        public static event EventHandler<OperationsGetList> GetListOperations;

        public static void SetListOperations()
        {
            if (GetListOperations != null)
            {
                GetListOperations(null, new OperationsGetList());
            }
        }

        #endregion

        #region дает знать что денег на счету достаточно для оплаты

        public static event EventHandler<FlagGetPurse> GetFlagPurse;

        public static void SetFlagPurse()
        {
            if (GetFlagPurse != null)
            {
                GetFlagPurse(null, new FlagGetPurse());
            }
        }

        #endregion

        #region дает все контролам знать что произошла оплата и нужно обновить данные

        public static event EventHandler<FlagGetRefresh> GetFlagRefresh;

        public static void SetFlagRefresh(List<NewInfoMainElement> mainElement)
        {
            if (GetFlagRefresh != null)
            {
                GetFlagRefresh(null, new FlagGetRefresh(mainElement));
            }
        }

        #endregion

        #region обновляет главное окно категорий

        public static event EventHandler<FlagGetMainManuItems> GetFlagMainManuItems;

        public static void SetFlagMainManuItems()
        {
            if (GetFlagMainManuItems != null)
            {
                GetFlagMainManuItems(null, new FlagGetMainManuItems());
            }
        }

        #endregion

        #region возвращает лист всех счетов " Кошелек " и т.д.

        public static event EventHandler<GetListCategori> GetListCategoriIvent;

        public static void SetListCategori(List<string> list)
        {
            if (GetListCategoriIvent != null)
            {
                GetListCategoriIvent(null, new GetListCategori(list));
            }
        }

        #endregion

        #region говорил контролу отвечающему за средства понять что ему нужно вернуть список категорий

        public static event EventHandler<GetListCategoriF> GetListCategoriF;

        public static void SetListCategoriF()
        {
            if (GetListCategoriF != null)
            {
                GetListCategoriF(null, new GetListCategoriF());
            }
        }

        #endregion
    }
    public class PurseGetSize : EventArgs
    {
        public string TextTransaction { get; set; }
        public double SizeTransaction { get; set; }

        public PurseGetSize(string textTransaction, double sizeTransaction)
        {
            this.TextTransaction = textTransaction;
            this.SizeTransaction = sizeTransaction;
        }
    }
    public class InfoGetTopText : EventArgs
    {
        public string TextTransaction { get; set; }

        public InfoGetTopText(string textTransaction)
        {
            this.TextTransaction = textTransaction;
        }
    }
    public class OperationsGetList : EventArgs
    {
        public OperationsGetList()
        {
        }
    }
    public class FlagGetPurse : EventArgs
    {
        public FlagGetPurse()
        {
        }
    }
    public class FlagGetRefresh : EventArgs
    {
        public List<NewInfoMainElement> MainElement { get; set; }

        public FlagGetRefresh(List<NewInfoMainElement> mainElement)
        {
            this.MainElement = mainElement;
        }
    }
    public class FlagGetMainManuItems : EventArgs
    {
        public FlagGetMainManuItems() { }
    }
    public class GetListCategori : EventArgs
    {
        public List<string> List;

        public GetListCategori(List<string> list)
        {
            List = list;
        }
    }
    public class GetListCategoriF : EventArgs
    {
        public GetListCategoriF() { }
    }
}
