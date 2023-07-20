using System.Collections.Generic;
using System.ComponentModel;

namespace HDGraph
{
    public class DirNodeSortableBindingList : SortableBindingList<DirectoryNode>
    {
        public DirNodeSortableBindingList()
            : base()
        {
        }

        public DirNodeSortableBindingList(IList<DirectoryNode> list)
            : base(list)
        {
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            PropertyDescriptor propHrTotalSize = TypeDescriptor.GetProperties(
                            typeof(DirectoryNode)
                            ).Find("HumanReadableTotalSize", false);
            PropertyDescriptor propHrFilesSize = TypeDescriptor.GetProperties(
                            typeof(DirectoryNode)
                            ).Find("HumanReadableFilesSize", false);
            if (prop == propHrTotalSize)
            {
                PropertyDescriptor propTotalSize = TypeDescriptor.GetProperties(
                            typeof(DirectoryNode)
                            ).Find("TotalSize", false);
                base.ApplySortCore(propTotalSize, direction);
            }
            else if (prop == propHrFilesSize)
            {
                PropertyDescriptor propFilesSize = TypeDescriptor.GetProperties(
                            typeof(DirectoryNode)
                            ).Find("FilesSize", false);
                base.ApplySortCore(propFilesSize, direction);
            }
            else
            {
                base.ApplySortCore(prop, direction);
            }
        }
    }
}
