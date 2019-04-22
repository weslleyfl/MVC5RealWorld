using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld.Models.DB
{
    public class EmailLembreteSenha
    {
        public string UsuarioNome { get; set; }

        public string UsuarioSenha { get; set; }

        public DateTime DataSolicitacao { get; set; }
    }
}
