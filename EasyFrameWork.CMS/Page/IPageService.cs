/* http://www.zkea.net/ Copyright 2016 ZKEASOFT http://www.zkea.net/licenses */
using Easy.RepositoryPattern;

namespace Easy.Web.CMS.Page
{
    public interface IPageService : IService<PageEntity>
    {
        void Move(string id, int position, int oldPosition);
        PageEntity GetByPath(string path, bool isPreView);
        void MarkChanged(string pageId);
        void Publish(PageEntity item);
    }
}