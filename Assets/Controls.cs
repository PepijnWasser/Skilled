// GENERATED AUTOMATICALLY FROM 'Assets/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Main Menu"",
            ""id"": ""f5252762-b67f-4af1-ac90-78c773d632a2"",
            ""actions"": [
                {
                    ""name"": ""Open Settings"",
                    ""type"": ""Button"",
                    ""id"": ""51173680-515e-41f0-9ccf-5fe964906dea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f6d5d999-9224-4228-8d58-6f9e2bff7148"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard & mouse"",
                    ""action"": ""Open Settings"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Game"",
            ""id"": ""e28e2ec6-c45e-412f-a004-d0e722ed93da"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""1db3b131-9e06-41a5-9dd2-1927797aacac"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""77b7d05c-7c66-418f-84a7-760e27bef77a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Open Settings"",
                    ""type"": ""Button"",
                    ""id"": ""4755df8d-c6f6-40d0-b2b9-a955d2e7ac9c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Increase Volume"",
                    ""type"": ""Button"",
                    ""id"": ""ab8a0580-50f4-4bba-be35-379bea5902ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Decrease Volume"",
                    ""type"": ""Button"",
                    ""id"": ""10eee5d1-77dc-4644-ad2f-96c981508106"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b0131a4e-a340-4691-90d0-ae64941703f7"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard & mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""4b70861c-f82c-413c-8197-e1e0f39c6d31"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8e2a9c56-e9e6-4f76-98f0-b503ca0db214"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard & mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ae037efd-080a-4a06-aabe-a93cfa8538ad"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard & mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b002f012-c057-4829-b473-0277e437d54b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard & mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6f09fe67-9019-4bfc-a892-ac67fbbc59de"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard & mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""DPAD"",
                    ""id"": ""20ece6d0-e12f-4c1d-88a1-a01c3de8e7aa"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c4c2dafd-ef45-4602-af96-b12b2f40224a"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""39d7fac8-409f-4e2b-a856-d43b4a5b0a22"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""45e647e4-4b3b-424d-b094-d0a9b4873036"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bde01558-f06c-4039-b3d2-67459c7643b3"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a86c373f-a280-4135-b3ba-bfbef408e336"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard & mouse"",
                    ""action"": ""Open Settings"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""78645f4e-3e8c-4605-8143-7f1ffdb89e81"",
                    ""path"": ""<Keyboard>/numpadPlus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard & mouse"",
                    ""action"": ""Increase Volume"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81a39bad-af02-453d-bafe-0375a3719967"",
                    ""path"": ""<Keyboard>/minus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard & mouse"",
                    ""action"": ""Decrease Volume"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Focusable"",
            ""id"": ""4f069e3b-6042-4f2c-b486-c4fd7aced73b"",
            ""actions"": [
                {
                    ""name"": ""Leave"",
                    ""type"": ""Button"",
                    ""id"": ""57a0a472-31af-41ac-8ce8-6922cf5049f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Open Settings"",
                    ""type"": ""Button"",
                    ""id"": ""d879735e-9694-442e-828d-7f303e34050c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""49c96855-9a63-439a-81bb-968b6b3c3a10"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard & mouse"",
                    ""action"": ""Leave"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""967fafbc-d1c7-46e1-80c2-2bdedac353f5"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard & mouse"",
                    ""action"": ""Open Settings"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""InputField"",
            ""id"": ""fdd66ed9-2e63-4e81-9e55-dad106f719e2"",
            ""actions"": [],
            ""bindings"": []
        },
        {
            ""name"": ""Chat"",
            ""id"": ""49b08441-138b-4eee-8f57-a0ef8228b48f"",
            ""actions"": [
                {
                    ""name"": ""SendMessage"",
                    ""type"": ""Button"",
                    ""id"": ""9bf7a74d-2131-496b-9bbb-af41d54ad8ab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a4c6d83e-6805-40f3-a4a6-25e901f1078e"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard & mouse"",
                    ""action"": ""SendMessage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Rebind"",
            ""id"": ""1d725380-3750-4c0e-82e7-363b49c9b9dd"",
            ""actions"": [],
            ""bindings"": []
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""keyboard & mouse"",
            ""bindingGroup"": ""keyboard & mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""gamepad"",
            ""bindingGroup"": ""gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Main Menu
        m_MainMenu = asset.FindActionMap("Main Menu", throwIfNotFound: true);
        m_MainMenu_OpenSettings = m_MainMenu.FindAction("Open Settings", throwIfNotFound: true);
        // Game
        m_Game = asset.FindActionMap("Game", throwIfNotFound: true);
        m_Game_Movement = m_Game.FindAction("Movement", throwIfNotFound: true);
        m_Game_Interact = m_Game.FindAction("Interact", throwIfNotFound: true);
        m_Game_OpenSettings = m_Game.FindAction("Open Settings", throwIfNotFound: true);
        m_Game_IncreaseVolume = m_Game.FindAction("Increase Volume", throwIfNotFound: true);
        m_Game_DecreaseVolume = m_Game.FindAction("Decrease Volume", throwIfNotFound: true);
        // Focusable
        m_Focusable = asset.FindActionMap("Focusable", throwIfNotFound: true);
        m_Focusable_Leave = m_Focusable.FindAction("Leave", throwIfNotFound: true);
        m_Focusable_OpenSettings = m_Focusable.FindAction("Open Settings", throwIfNotFound: true);
        // InputField
        m_InputField = asset.FindActionMap("InputField", throwIfNotFound: true);
        // Chat
        m_Chat = asset.FindActionMap("Chat", throwIfNotFound: true);
        m_Chat_SendMessage = m_Chat.FindAction("SendMessage", throwIfNotFound: true);
        // Rebind
        m_Rebind = asset.FindActionMap("Rebind", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Main Menu
    private readonly InputActionMap m_MainMenu;
    private IMainMenuActions m_MainMenuActionsCallbackInterface;
    private readonly InputAction m_MainMenu_OpenSettings;
    public struct MainMenuActions
    {
        private @Controls m_Wrapper;
        public MainMenuActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @OpenSettings => m_Wrapper.m_MainMenu_OpenSettings;
        public InputActionMap Get() { return m_Wrapper.m_MainMenu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainMenuActions set) { return set.Get(); }
        public void SetCallbacks(IMainMenuActions instance)
        {
            if (m_Wrapper.m_MainMenuActionsCallbackInterface != null)
            {
                @OpenSettings.started -= m_Wrapper.m_MainMenuActionsCallbackInterface.OnOpenSettings;
                @OpenSettings.performed -= m_Wrapper.m_MainMenuActionsCallbackInterface.OnOpenSettings;
                @OpenSettings.canceled -= m_Wrapper.m_MainMenuActionsCallbackInterface.OnOpenSettings;
            }
            m_Wrapper.m_MainMenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @OpenSettings.started += instance.OnOpenSettings;
                @OpenSettings.performed += instance.OnOpenSettings;
                @OpenSettings.canceled += instance.OnOpenSettings;
            }
        }
    }
    public MainMenuActions @MainMenu => new MainMenuActions(this);

    // Game
    private readonly InputActionMap m_Game;
    private IGameActions m_GameActionsCallbackInterface;
    private readonly InputAction m_Game_Movement;
    private readonly InputAction m_Game_Interact;
    private readonly InputAction m_Game_OpenSettings;
    private readonly InputAction m_Game_IncreaseVolume;
    private readonly InputAction m_Game_DecreaseVolume;
    public struct GameActions
    {
        private @Controls m_Wrapper;
        public GameActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Game_Movement;
        public InputAction @Interact => m_Wrapper.m_Game_Interact;
        public InputAction @OpenSettings => m_Wrapper.m_Game_OpenSettings;
        public InputAction @IncreaseVolume => m_Wrapper.m_Game_IncreaseVolume;
        public InputAction @DecreaseVolume => m_Wrapper.m_Game_DecreaseVolume;
        public InputActionMap Get() { return m_Wrapper.m_Game; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameActions set) { return set.Get(); }
        public void SetCallbacks(IGameActions instance)
        {
            if (m_Wrapper.m_GameActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GameActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnMovement;
                @Interact.started -= m_Wrapper.m_GameActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnInteract;
                @OpenSettings.started -= m_Wrapper.m_GameActionsCallbackInterface.OnOpenSettings;
                @OpenSettings.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnOpenSettings;
                @OpenSettings.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnOpenSettings;
                @IncreaseVolume.started -= m_Wrapper.m_GameActionsCallbackInterface.OnIncreaseVolume;
                @IncreaseVolume.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnIncreaseVolume;
                @IncreaseVolume.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnIncreaseVolume;
                @DecreaseVolume.started -= m_Wrapper.m_GameActionsCallbackInterface.OnDecreaseVolume;
                @DecreaseVolume.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnDecreaseVolume;
                @DecreaseVolume.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnDecreaseVolume;
            }
            m_Wrapper.m_GameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @OpenSettings.started += instance.OnOpenSettings;
                @OpenSettings.performed += instance.OnOpenSettings;
                @OpenSettings.canceled += instance.OnOpenSettings;
                @IncreaseVolume.started += instance.OnIncreaseVolume;
                @IncreaseVolume.performed += instance.OnIncreaseVolume;
                @IncreaseVolume.canceled += instance.OnIncreaseVolume;
                @DecreaseVolume.started += instance.OnDecreaseVolume;
                @DecreaseVolume.performed += instance.OnDecreaseVolume;
                @DecreaseVolume.canceled += instance.OnDecreaseVolume;
            }
        }
    }
    public GameActions @Game => new GameActions(this);

    // Focusable
    private readonly InputActionMap m_Focusable;
    private IFocusableActions m_FocusableActionsCallbackInterface;
    private readonly InputAction m_Focusable_Leave;
    private readonly InputAction m_Focusable_OpenSettings;
    public struct FocusableActions
    {
        private @Controls m_Wrapper;
        public FocusableActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Leave => m_Wrapper.m_Focusable_Leave;
        public InputAction @OpenSettings => m_Wrapper.m_Focusable_OpenSettings;
        public InputActionMap Get() { return m_Wrapper.m_Focusable; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FocusableActions set) { return set.Get(); }
        public void SetCallbacks(IFocusableActions instance)
        {
            if (m_Wrapper.m_FocusableActionsCallbackInterface != null)
            {
                @Leave.started -= m_Wrapper.m_FocusableActionsCallbackInterface.OnLeave;
                @Leave.performed -= m_Wrapper.m_FocusableActionsCallbackInterface.OnLeave;
                @Leave.canceled -= m_Wrapper.m_FocusableActionsCallbackInterface.OnLeave;
                @OpenSettings.started -= m_Wrapper.m_FocusableActionsCallbackInterface.OnOpenSettings;
                @OpenSettings.performed -= m_Wrapper.m_FocusableActionsCallbackInterface.OnOpenSettings;
                @OpenSettings.canceled -= m_Wrapper.m_FocusableActionsCallbackInterface.OnOpenSettings;
            }
            m_Wrapper.m_FocusableActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Leave.started += instance.OnLeave;
                @Leave.performed += instance.OnLeave;
                @Leave.canceled += instance.OnLeave;
                @OpenSettings.started += instance.OnOpenSettings;
                @OpenSettings.performed += instance.OnOpenSettings;
                @OpenSettings.canceled += instance.OnOpenSettings;
            }
        }
    }
    public FocusableActions @Focusable => new FocusableActions(this);

    // InputField
    private readonly InputActionMap m_InputField;
    private IInputFieldActions m_InputFieldActionsCallbackInterface;
    public struct InputFieldActions
    {
        private @Controls m_Wrapper;
        public InputFieldActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_InputField; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InputFieldActions set) { return set.Get(); }
        public void SetCallbacks(IInputFieldActions instance)
        {
            if (m_Wrapper.m_InputFieldActionsCallbackInterface != null)
            {
            }
            m_Wrapper.m_InputFieldActionsCallbackInterface = instance;
            if (instance != null)
            {
            }
        }
    }
    public InputFieldActions @InputField => new InputFieldActions(this);

    // Chat
    private readonly InputActionMap m_Chat;
    private IChatActions m_ChatActionsCallbackInterface;
    private readonly InputAction m_Chat_SendMessage;
    public struct ChatActions
    {
        private @Controls m_Wrapper;
        public ChatActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @SendMessage => m_Wrapper.m_Chat_SendMessage;
        public InputActionMap Get() { return m_Wrapper.m_Chat; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ChatActions set) { return set.Get(); }
        public void SetCallbacks(IChatActions instance)
        {
            if (m_Wrapper.m_ChatActionsCallbackInterface != null)
            {
                @SendMessage.started -= m_Wrapper.m_ChatActionsCallbackInterface.OnSendMessage;
                @SendMessage.performed -= m_Wrapper.m_ChatActionsCallbackInterface.OnSendMessage;
                @SendMessage.canceled -= m_Wrapper.m_ChatActionsCallbackInterface.OnSendMessage;
            }
            m_Wrapper.m_ChatActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SendMessage.started += instance.OnSendMessage;
                @SendMessage.performed += instance.OnSendMessage;
                @SendMessage.canceled += instance.OnSendMessage;
            }
        }
    }
    public ChatActions @Chat => new ChatActions(this);

    // Rebind
    private readonly InputActionMap m_Rebind;
    private IRebindActions m_RebindActionsCallbackInterface;
    public struct RebindActions
    {
        private @Controls m_Wrapper;
        public RebindActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_Rebind; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RebindActions set) { return set.Get(); }
        public void SetCallbacks(IRebindActions instance)
        {
            if (m_Wrapper.m_RebindActionsCallbackInterface != null)
            {
            }
            m_Wrapper.m_RebindActionsCallbackInterface = instance;
            if (instance != null)
            {
            }
        }
    }
    public RebindActions @Rebind => new RebindActions(this);
    private int m_keyboardmouseSchemeIndex = -1;
    public InputControlScheme keyboardmouseScheme
    {
        get
        {
            if (m_keyboardmouseSchemeIndex == -1) m_keyboardmouseSchemeIndex = asset.FindControlSchemeIndex("keyboard & mouse");
            return asset.controlSchemes[m_keyboardmouseSchemeIndex];
        }
    }
    private int m_gamepadSchemeIndex = -1;
    public InputControlScheme gamepadScheme
    {
        get
        {
            if (m_gamepadSchemeIndex == -1) m_gamepadSchemeIndex = asset.FindControlSchemeIndex("gamepad");
            return asset.controlSchemes[m_gamepadSchemeIndex];
        }
    }
    public interface IMainMenuActions
    {
        void OnOpenSettings(InputAction.CallbackContext context);
    }
    public interface IGameActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnOpenSettings(InputAction.CallbackContext context);
        void OnIncreaseVolume(InputAction.CallbackContext context);
        void OnDecreaseVolume(InputAction.CallbackContext context);
    }
    public interface IFocusableActions
    {
        void OnLeave(InputAction.CallbackContext context);
        void OnOpenSettings(InputAction.CallbackContext context);
    }
    public interface IInputFieldActions
    {
    }
    public interface IChatActions
    {
        void OnSendMessage(InputAction.CallbackContext context);
    }
    public interface IRebindActions
    {
    }
}
