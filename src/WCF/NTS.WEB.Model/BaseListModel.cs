
namespace NTS.WEB.Model
{
    /// <summary>
    /// 查询数据的底层modle
    /// </summary>
    public class BaseListModel
    {
        /// <summary>
        /// Page
        /// </summary>
        public int Page
        {
            get;
            set;
        }

        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }
        
        /// <summary>
        /// 对象ID
        /// </summary>
        public int ObjectId
        {
            get;
            set;
        }

        /// <summary>
        /// 对象分类分项
        /// </summary>
        public int CategoryId
        {
            get;
            set;
        }
    }
}
