public interface IEnemyAI
{
    // Este es el "contrato". Todo enemigo que use esta interfaz 
    // estará obligado a tener una función para encender/apagar su cerebro.
    void ToggleAI(bool isEnabled);
}