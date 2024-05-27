﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramCarInsurance.Domain.Abstractions
{
    public interface ICommandExecutor
    {
        public Task GetUpdate(Update update);
    }

}
