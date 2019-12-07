using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvents : MonoBehaviour
{
    public enum EventsKeys 
    {
        MainMenu_Start, Gameplay_Start, Versus_Animation_Start, VersusUI_Show, PauseMenu_Show, Resume_Pressed, Rematch_Pressed,
        Cookies_Screen_Show, Spell_Selection_Screen_Show, MainMenu_Pressed,

        UI_Move, UI_Select,

        Fly_Tutorial_Start, Player_Collision_Portal, Meg_Walk, Zak_Walk, Player_Jump, Player_On_Ground, Player_Flying, Player_Invoking,
        Player_Death, Player_Out_Of_Bounds_Alarm, Player_Out_Of_Bounds_Death, Player_Victory,

        BaseballBat, BaseballBat_Hit, BubbleBurst, BubbleBurst_Hit, FlamingFist, FlamingFist_Hit,
        HastyHurricane, HastyHurricane_Hit, Hamerfall, Hamerfall_Hit, MagicMissile, MagicMissile_Hit, IceStake, IceStake_Hit,
        MagicPunch, MagicPunch_Hit, Rock, Rock_Hit, SummonerSlap, SummonerSlap_Hit,
        WindShuriken, WindShuriken_Hit, WindShuriken_Going_Back, WindPush, WindPush_Hit,

        Player_Stun, Player_On_Fire, Player_KnockBack, Player_Freezed
    }

    static public Dictionary<string, string> eventsIDs = new Dictionary<string, string>();

    private void Awake()
    {
        //Esto deberia ser UI pero los de audio son unos pelotudos
        eventsIDs.Add("MainMenu_Start", "Inicia_menu");
        eventsIDs.Add("Gameplay_Start", "Inicia_gameplay");
        eventsIDs.Add("Versus_Animation_Start", "Versus");
        eventsIDs.Add("VersusUI_Show", "Pantalla_versus");
        eventsIDs.Add("PauseMenu_Show", "Pause");
        eventsIDs.Add("Resume_Pressed", "Resume");
        eventsIDs.Add("Rematch_Pressed", "Rematch");
        eventsIDs.Add("Cookies_Screen_Show", "Cookies_screen");
        eventsIDs.Add("Spell_Selection_Screen_Show", "Pantalla_poderes");
        eventsIDs.Add("MainMenu_Pressed", "Main_menu");
        //UI
        eventsIDs.Add("UI_Move", "UI_move");
        eventsIDs.Add("UI_Select", "UI_select");
        //Gameplay
        eventsIDs.Add("Fly_Tutorial_Start", "Inicia_tutorial");
        eventsIDs.Add("Player_Collision_Portal", "Atraviesa_portal");
        eventsIDs.Add("Meg_Walk", "Camina_meg");
        eventsIDs.Add("Zak_Walk", "Camina_zak");
        eventsIDs.Add("Player_Jump", "Salta");
        eventsIDs.Add("Player_On_Ground", "Cae_a_tierra");
        eventsIDs.Add("Player_Flying", "Vuela");
        eventsIDs.Add("Player_Invoking", "Carga_poder");
        eventsIDs.Add("Player_Death", "Explota");
        eventsIDs.Add("Player_Out_Of_Bounds_Alarm", "Alarma_vuela_alto");
        eventsIDs.Add("Player_Out_Of_Bounds_Death", "Cae_explota");
        eventsIDs.Add("Player_Victory", "Victoria");

        //Poderes
        eventsIDs.Add("BaseballBat", "Bate");
        eventsIDs.Add("BaseballBat_Hit", "Bate_acierta");

        eventsIDs.Add("BubbleBurst", "Bubble");
        eventsIDs.Add("BubbleBurst_Hit", "Bubble_acierta");

        eventsIDs.Add("FlamingFist", "Fire");
        eventsIDs.Add("FlamingFist_Hit", "Fire_acierta");

        eventsIDs.Add("HastyHurricane", "Hurricane");
        eventsIDs.Add("HastyHurricane_Hit", "Hurricane_acierta");

        eventsIDs.Add("Hamerfall", "Martillo");
        eventsIDs.Add("Hamerfall_Hit", "Martillo_acierta");

        eventsIDs.Add("MagicMissile", "Misil");
        eventsIDs.Add("MagicMissile_Hit", "Misil_acierta");

        eventsIDs.Add("IceStake", "Polar");
        eventsIDs.Add("IceStake_Hit", "Polar_acierta");

        eventsIDs.Add("MagicPunch", "Pummel");
        eventsIDs.Add("MagicPunch_Hit", "Pummel_acierta");

        eventsIDs.Add("Rock", "Roca");
        eventsIDs.Add("Rock_Hit", "Roca_acierta");

        eventsIDs.Add("SummonerSlap", "Slap");
        eventsIDs.Add("SummonerSlap_Hit", "Slap_acierta");

        eventsIDs.Add("WindShuriken", "Surikan");
        eventsIDs.Add("WindShuriken_Hit", "Surikan_acierta");
        eventsIDs.Add("WindShuriken_Going_Back", "Surikan_vuelve");

        eventsIDs.Add("WindPush", "Viento");
        eventsIDs.Add("WindPush_Hit", "Viento_acierta");

        //Efectos en  el contricante
        eventsIDs.Add("Player_Stun", "Pajaritos");
        eventsIDs.Add("Player_On_Fire", "Prende_fuego");
        eventsIDs.Add("Player_KnockBack", "Arrastra");
        eventsIDs.Add("Player_Freezed", "Congela");
    }
}
