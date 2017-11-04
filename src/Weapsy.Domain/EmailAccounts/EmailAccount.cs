﻿using FluentValidation;
using System;
using Weapsy.Framework.Domain;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Domain.EmailAccounts.Events;

namespace Weapsy.Domain.EmailAccounts
{
    public class EmailAccount : AggregateRoot
    {
        public Guid SiteId { get; private set; }
        public string Address { get; private set; }
        public string DisplayName { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public bool DefaultCredentials { get; private set; }
        public bool Ssl { get; private set; }
        public EmailAccountStatus Status { get; private set; }

        public EmailAccount() { }

        private EmailAccount(CreateEmailAccountCommand cmd) : base(cmd.Id)
        {
            SiteId = cmd.SiteId;
            Address = cmd.Address;
            DisplayName = cmd.DisplayName;
            Host = cmd.Host;
            Port = cmd.Port;
            UserName = cmd.UserName;
            Password = cmd.Password;
            DefaultCredentials = cmd.DefaultCredentials;
            Ssl = cmd.Ssl;
            Status = EmailAccountStatus.Active;

            AddEvent(new EmailAccountCreatedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Address = Address,
                DisplayName = DisplayName,
                Host = Host,
                Port = Port,
                UserName = UserName,
                Password = Password,
                DefaultCredentials = DefaultCredentials,
                Ssl = Ssl,
                Status = Status
            });
        }

        public static EmailAccount CreateNew(CreateEmailAccountCommand cmd, IValidator<CreateEmailAccountCommand> validator)
        {
            validator.ValidateCommand(cmd);

            return new EmailAccount(cmd);
        }

        public void UpdateDetails(UpdateEmailAccountDetailsCommand cmd, IValidator<UpdateEmailAccountDetailsCommand> validator)
        {
            validator.ValidateCommand(cmd);

            Address = cmd.Address;
            DisplayName = cmd.DisplayName;
            Host = cmd.Host;
            Port = cmd.Port;
            UserName = cmd.UserName;
            Password = cmd.Password;
            DefaultCredentials = cmd.DefaultCredentials;
            Ssl = cmd.Ssl;

            AddEvent(new EmailAccountDetailsUpdatedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Address = Address,
                DisplayName = DisplayName,
                Host = Host,
                Port = Port,
                UserName = UserName,
                Password = Password,
                DefaultCredentials = DefaultCredentials,
                Ssl = Ssl
            });
        }

        public void Delete(DeleteEmailAccountCommand cmd, IValidator<DeleteEmailAccountCommand> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == EmailAccountStatus.Deleted)
                throw new Exception("Email account already deleted.");

            Status = EmailAccountStatus.Deleted;

            AddEvent(new EmailAccountDeletedEvent
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }
    }
}
