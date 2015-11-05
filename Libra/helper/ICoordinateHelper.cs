using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Libra.helper
{
    /// <summary>
    /// 坐标网格换算的帮助接口
    /// </summary>
    public interface ICoordinateHelper
    {
        double Width { get; set; }

        double Height { get; set; }

        Point TopPoint { get; set; }

        /// <summary>
        /// 获得鼠标位置所在方块的索引值
        /// </summary>
        /// <param name="mouseP"></param>
        /// <returns></returns>
        Point GetItemIndex(Point mouseP);

        /// <summary>
        /// 根据方块的索引值获取方块的屏幕坐标
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        Point GetItemPos(int row, int col);
    }
}
