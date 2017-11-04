using System.Collections.Generic;
using Weapsy.Domain.Pages.Commands;
using System;
using FluentValidation;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Events;

namespace Weapsy.Domain.Pages.Handlers
{
    public class ReorderPageModulesHandler : ICommandHandler<ReorderPageModulesCommand>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<ReorderPageModulesCommand> _validator;

        public ReorderPageModulesHandler(IPageRepository pageRepository, IValidator<ReorderPageModulesCommand> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public IEnumerable<IEvent> Handle(ReorderPageModulesCommand cmd)
        {
            var page = _pageRepository.GetById(cmd.SiteId, cmd.PageId);

            if (page == null)
                throw new Exception("Page not found");

            page.ReorderPageModules(cmd, _validator);

            _pageRepository.Update(page);

            return page.Events;
        }
    }
}
