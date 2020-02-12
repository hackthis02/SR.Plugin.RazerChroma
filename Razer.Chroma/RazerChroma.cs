using UnityEngine;
using System.Collections.Generic;
using Corale.Colore.Core;
using Corale.Colore.Razer.Keyboard;
using System;
using System.Linq;

/// Example Satellite Reign pause mod.
/// </summary>
public class RazerChroma : ISrPlugin
{
    List<AgentAI> selected_agents;
    bool firstinit = true;
    private readonly Corale.Colore.Core.Color magenta = new Corale.Colore.Core.Color(128, 0, 128);
    KeyRemapper KRemap = Manager.GetKeyRemapper();
    private Dictionary<Key, float> refreshTimers = new Dictionary<Key, float>();

    /// <summary>
    /// Plugin initialization 
    /// </summary>
    public void Initialize()
    {
        UnityEngine.Debug.Log("Initializing Razer Chroma support");
        selected_agents = new List<AgentAI>();
    }


    /// <summary>
    /// Called once per frame.
    /// </summary>
    public void Update()
    {
        if (Manager.Get().GameInProgress)
        {
            if (firstinit)
            {
                Keyboard.Instance.SetAll(magenta);
                firstinit = false;
            }
            foreach (AgentAI a in AgentAI.GetAgents())
            {
                Key keycode = a.GetSelectionKey().ToKey();

                if (a.IsInCombat())
                {
                    Keyboard.Instance[keycode] = UnityEngine.Color.red.ToColore();
                }
                else if (a.GetHealthState() != Health.HealthState.alive)
                {
                    UpdateKeyColor(keycode, a.GetRespawnTimePct() / 0.5f);
                }
                else if (a.GetHealthState() == Health.HealthState.beingRevived)
                {
                    Keyboard.Instance[keycode] = UnityEngine.Color.blue.ToColore();
                }
                else if (a.Selectable)
                {
                    if (a.IsSelected())
                    {
                        if (!selected_agents.Contains(a))
                            selected_agents.Add(a);

                        if (a.GetCurrentWeapon(false) != null)
                            Keyboard.Instance[keycode] = new Corale.Colore.Core.Color(255, 128, 0);
                        else
                            Keyboard.Instance[keycode] = UnityEngine.Color.green.ToColore();

                        SelectAbilities(a);
                    }
                    else
                    {
                        if (selected_agents.Contains(a))
                        {
                            DeselectAbilities(a);
                            selected_agents.Remove(a);
                        }
                        if (a.GetCurrentWeapon(false) != null)
                            Keyboard.Instance[keycode] = new Corale.Colore.Core.Color(255, 128, 0);
                        else
                            Keyboard.Instance[keycode] = UnityEngine.Color.blue.ToColore();
                    }
                }
            }

            if (selected_agents.Count > 0)
                Keyboard.Instance[KeyCode.F.ToKey()] = UnityEngine.Color.white.ToColore();
            else
                Keyboard.Instance[KeyCode.F.ToKey()] = magenta;
        }
        else
        {
            Keyboard.Instance.SetAll(new Corale.Colore.Core.Color(57, 255, 20));
            firstinit = true;
        }
    }

    private void UpdateKeyColor(Key key, float pct)
    {
        UnityEngine.Color color = UnityEngine.Color.Lerp(UnityEngine.Color.red, UnityEngine.Color.black, pct);
        Keyboard.Instance[key] = color.ToColore();
    }
    void DeselectAbilities(AgentAI a)
    {
        string[] aNames; int[] aIds;
        a.GetAbilities().m_AbilityManager.GetAbilityNamesAndIDs(out aIds, out aNames);
        for (int i = 0; i < aIds.Length; i++)
        {
            if (a.GetAbilities() != null && a.GetAbilities().GetAbility(aIds[i]) != null)
            {
                try
                {
                    if (a.GetAbilities().HasAbility(aIds[i]))
                    {
                        Ability abil = a.GetAbilities().GetAbility(aIds[i]);
                        KeyCode keycode = KeyRemapper.GetMappingForAbility(aIds[i]).m_Key;
                        Keyboard.Instance[keycode.ToKey()] = magenta;
                    }
                }
                catch { }
            }
        }
    }

    void SelectAbilities(AgentAI a)
    {
        string[] aNames; int[] aIds;
        a.GetAbilities().m_AbilityManager.GetAbilityNamesAndIDs(out aIds, out aNames);
        for (int i = 0; i < aIds.Length; i++)
        {
            if (a.GetAbilities() != null && a.GetAbilities().GetAbility(aIds[i]) != null)
            {
                try
                {
                    if (a.GetAbilities().HasAbility(aIds[i]))
                    {
                        Ability abil = a.GetAbilities().GetAbility(aIds[i]);
                        KeyCode keycode = KeyRemapper.GetMappingForAbility(aIds[i]).m_Key;

                        switch (abil.State)
                        {
                            case Ability.AbilityState.prepped:
                                Keyboard.Instance[keycode.ToKey()] = UnityEngine.Color.yellow.ToColore();
                                break;
                            case Ability.AbilityState.activating:
                                Keyboard.Instance[keycode.ToKey()] = UnityEngine.Color.gray.ToColore();
                                break;
                            case Ability.AbilityState.active:
                                Keyboard.Instance[keycode.ToKey()] = UnityEngine.Color.green.ToColore();
                                break;
                            case Ability.AbilityState.ready:
                                Keyboard.Instance[keycode.ToKey()] = UnityEngine.Color.white.ToColore();
                                break;
                            default:
                                Keyboard.Instance[keycode.ToKey()] = magenta;
                                break;
                        }

                        if (abil.GetCooldownPercentage() > 0)
                        {
                            Keyboard.Instance[keycode.ToKey()] = a.IsSelected() ? UnityEngine.Color.yellow.ToColore() : magenta;
                        }
                        else if (abil.GetAmmoCount() == 0 && abil.GetMaxAmmo() != 0 && abil.GetCooldownPercentage() > 0)
                        {
                            Keyboard.Instance[keycode.ToKey()] = a.IsSelected() ? UnityEngine.Color.red.ToColore() : magenta;
                        }
                    }

                }
                catch { }
            }
        }
    }

    public string GetName()
    {
        return "Razer Chroma support";
    }
}

public static class Extensions
{
    public static Corale.Colore.Core.Color ToColore(this UnityEngine.Color source)
    {
        return new Corale.Colore.Core.Color(source.r, source.g, source.b);
    }

    public static Key ToKey(this KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.Alpha0: return Key.D0;
            case KeyCode.Alpha1: return Key.D1;
            case KeyCode.Alpha2: return Key.D2;
            case KeyCode.Alpha3: return Key.D3;
            case KeyCode.Alpha4: return Key.D4;
            case KeyCode.Alpha5: return Key.D5;
            case KeyCode.Alpha6: return Key.D6;
            case KeyCode.Alpha7: return Key.D7;
            case KeyCode.Alpha8: return Key.D8;
            case KeyCode.Alpha9: return Key.D9;
            case KeyCode.F: return Key.F;
            case KeyCode.H: return Key.H;
            case KeyCode.V: return Key.V;
            case KeyCode.Q: return Key.Q;
            case KeyCode.W: return Key.W;
            case KeyCode.E: return Key.E;
            case KeyCode.R: return Key.R;
            case KeyCode.T: return Key.T;
            case KeyCode.Y: return Key.Y;
            case KeyCode.U: return Key.U;
            case KeyCode.I: return Key.I;
            case KeyCode.O: return Key.O;
            case KeyCode.P: return Key.P;
            case KeyCode.A: return Key.A;
            case KeyCode.S: return Key.S;
            case KeyCode.D: return Key.D;
            case KeyCode.G: return Key.G;
            case KeyCode.J: return Key.J;
            case KeyCode.K: return Key.K;
            case KeyCode.L: return Key.L;
            case KeyCode.Z: return Key.Z;
            case KeyCode.X: return Key.X;
            case KeyCode.C: return Key.C;
            case KeyCode.B: return Key.B;
            case KeyCode.N: return Key.N;
            case KeyCode.M: return Key.M;
        }
        return Key.Escape;
    }
}