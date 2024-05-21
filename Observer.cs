using System;
using System.Collections.Generic;
using System.Threading;

namespace PatronesDeDiseño.Observer
{
    public interface IObservador
    {
        // Recibir actualización del sujeto
        void Actualizar(ISubject sujeto);
    }

    public interface ISubject
    {
        // Adjuntar un observador al sujeto.
        void Adjuntar(IObservador observador);

        // Separar un observador del sujeto.
        void Separar(IObservador observador);

        // Notificar a todos los observadores sobre un evento.
        void Notificar();
    }

    // El Sujeto posee un estado importante y notifica a los observadores cuando el
    // estado cambia.
    public class Sujeto : ISubject
    {
        // Por simplicidad, el estado del Sujeto, esencial para todos los suscriptores, se almacena en esta variable.
        public int Estado { get; set; } = -0;

        // Lista de suscriptores. En la vida real, la lista de suscriptores se puede almacenar de manera más completa (categorizada por tipo de evento, etc.).
        private List<IObservador> _observadores = new List<IObservador>();

        // Los métodos de gestión de suscripción.
        public void Adjuntar(IObservador observador)
        {
            Console.WriteLine("Sujeto: Se ha adjuntado un observador.");
            this._observadores.Add(observador);
        }

        public void Separar(IObservador observador)
        {
            this._observadores.Remove(observador);
            Console.WriteLine("Sujeto: Se ha separado un observador.");
        }

        // Desencadenar una actualización en cada suscriptor.
        public void Notificar()
        {
            Console.WriteLine("Sujeto: Notificando a los observadores...");

            foreach (var observador in _observadores)
            {
                observador.Actualizar(this);
            }
        }

        // Por lo general, la lógica de suscripción es solo una fracción de lo que un Sujeto
        // puede hacer realmente. Los sujetos comúnmente contienen algo de lógica de negocio importante,
        // que desencadena un método de notificación cada vez que algo importante está
        // a punto de suceder (o después).
        public void AlgunaLogicaDeNegocio()
        {
            Console.WriteLine("\nSujeto: Estoy haciendo algo importante.");
            this.Estado = new Random().Next(0, 10);

            Thread.Sleep(15);

            Console.WriteLine("Sujeto: Mi estado acaba de cambiar a: " + this.Estado);
            this.Notificar();
        }
    }

    // Los Observadores concretos reaccionan a las actualizaciones emitidas por el Sujeto al que habían
    // sido adjuntos.
    class ObservadorConcretoA : IObservador
    {
        public void Actualizar(ISubject sujeto)
        {
            if ((sujeto as Sujeto).Estado < 3)
            {
                Console.WriteLine("Observador A: Reaccionó al evento.");
            }
        }
    }

    class ObservadorConcretoB : IObservador
    {
        public void Actualizar(ISubject sujeto)
        {
            if ((sujeto as Sujeto).Estado == 0 || (sujeto as Sujeto).Estado >= 2)
            {
                Console.WriteLine("Observador B: Reaccionó al evento.");
            }
        }
    }

    class Programa
    {
        static void Main(string[] args)
        {
            // El código del cliente.
            var sujeto = new Sujeto();
            var observadorA = new ObservadorConcretoA();
            sujeto.Adjuntar(observadorA);

            var observadorB = new ObservadorConcretoB();
            sujeto.Adjuntar(observadorB);

            sujeto.AlgunaLogicaDeNegocio();
            sujeto.AlgunaLogicaDeNegocio();

            sujeto.Separar(observadorB);

            sujeto.AlgunaLogicaDeNegocio();
        }
    }
}
