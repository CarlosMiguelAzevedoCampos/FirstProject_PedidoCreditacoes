﻿using CMA.ISMAI.Sagas.Engine.ISMAI.Model;
using System.Collections.Generic;

namespace CMA.ISMAI.Engine.Automation.Sagas
{
    public interface ICreditacoesService
    {
        bool GetCardState(string cardId);
        string PostNewCard(CardDto card);
        List<string> GetCardAttachments(string cardId, int boardId);
    }
}
