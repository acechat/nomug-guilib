using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GUILib;

/// <summary>
/// This overlay displays all messages that the console receives.
/// </summary>
/// <author>Henning Vreyborg (marshen)</author>
/// <author>$LastChangedBy: marshen $</author>
/// <version>$Rev: 285 $, $Date: 2011-07-27 17:33:48 +0200 (Mi, 27 Jul 2011) $</version>
public class ConsoleOverlay : AConsoleOverlay
{
	/// <summary>
	/// Contains the last log messages received.
	/// </summary>
	private ConsoleData consoleData;
	
	private UnityEngine.Rect resolution;
	/// <summary>
	/// If true the next repaint will update the message label positions.
	/// </summary>
	private bool updateMessageLabels;
	private bool scrollToBottom; // WORKAROUND: can't set scroll position after new positioning because maximum scroll position is calculated next ongui.
	private string[] suggestedCommands;
	private int suggestedCommandIndex;
	
	private Style style;
	
	private Button button;
	private EditBox editbox;
	private Label editboxHelp;
	private Image background;
	private Box filterBox;
	private Box suggestionBox;
	private ScrollBox scrollBox;
	private Logger[] loggers;
	private List<Label> messageLabels;
	private List<Button> suggestionButtons;
	
	private const string EDITBOX_TEXT = "Enter your command here...";
	
	/// <summary>
	/// Construct the ConsoleOverlay 
	/// </summary>
	public ConsoleOverlay(ConsoleData consoleData) : base()
	{
		this.consoleData = consoleData;
		
		this.Visible = false;
		this.KeyUp += this.handleKeyEvent;
		
		this.messageLabels = new List<Label>();
		this.suggestionButtons = new List<Button>();
		
		style = new Style();
		
		style.TextColor = Color.white;
		
		Texture2D lightBackground = new Texture2D(5, 5);
		Texture2D darkBackground = new Texture2D(lightBackground.width, lightBackground.height);
		for (int x = 0; x < lightBackground.width; x++)
		{
			for (int y = 0; y < lightBackground.height; y++)
			{
				if (x == 0 || x == lightBackground.width - 1 || y == 0 || y == lightBackground.height- 1)
				{
					lightBackground.SetPixel(x, y, new Color(1f, 0.6f, 0f, 0.75f));
					darkBackground.SetPixel(x, y, new Color(1f, 0.6f, 0f, 0.75f));
				}
				else
				{
					if (x == 1 || x == lightBackground.width - 2 || y == 1 || y == lightBackground.height - 2)
					{
						lightBackground.SetPixel(x, y, new Color(0.2f, 0.2f, 0.2f, 0.75f));
						darkBackground.SetPixel(x, y, new Color(0.5f, 0.3f, 0f, 0.75f));
					}
					else
					{
						lightBackground.SetPixel(x, y, new Color(0.5f, 0.3f, 0f, 0.75f));
						darkBackground.SetPixel(x, y, new Color(0f, 0f, 0f, 0.75f));
					}
				}
			}
		}
		lightBackground.Apply();
		darkBackground.Apply();
		
		style.Background = darkBackground;
		style.Hover.Background = lightBackground;
		
		style.SetBorder(2);
		
		Texture2D backgroundTexture = new Texture2D(1, 1);
		backgroundTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f));
		backgroundTexture.Apply();
		
		this.background = new Image();
		this.background.Texture = backgroundTexture;
		
		this.button = new Button();
		this.button.Width = 100f;
		this.button.Height = 20f;
		this.button.ElementStyle.Alignment = TextAnchor.MiddleCenter;
		this.button.Name = "Filter";
		this.button.Text = "Filter";
		this.button.Click += this.execute;
		this.button.ClassStyle = this.style;

		this.editbox = new EditBox();
		this.editbox.Name = "EditBox ConsolveOverlay";
		this.editbox.Left = 5f;
		this.editbox.Height = 20f;
		this.editbox.ElementStyle.Alignment = TextAnchor.MiddleLeft;
		this.editbox.ElementStyle.PaddingLeft = this.editbox.ElementStyle.PaddingRight = 2;
		this.editbox.Click += this.handleEditBoxClear;
		this.editbox.Changed += this.handleOnChange;
		this.editbox.ClassStyle = this.style;
		
		this.editboxHelp = new Label();
		this.editboxHelp.Left = this.editbox.Left + this.editbox.ElementStyle.PaddingLeft.Value;
		this.editboxHelp.Height = this.editbox.Height;
		this.editboxHelp.Text = EDITBOX_TEXT;
		this.editboxHelp.ElementStyle.TextColor = Color.white;
		this.editboxHelp.ElementStyle.Alignment = TextAnchor.MiddleLeft;
		
		this.scrollBox = new ScrollBox();
		this.scrollBox.ElementStyle.BackgroundPath = "empty";
		this.scrollBox.ElementStyle.Left = 5f; // also used as right margin
		this.scrollBox.ElementStyle.Top = 5f; // used as bottom margin
		this.scrollBox.ScrollBar = GUILib.Types.ScrollBar.VERTICAL;
		this.scrollBox.VerticalScrollBarStyle.Width = 15f;
		
		this.filterBox = new Box();
		this.filterBox.Visible = false;
		this.filterBox.ClassStyle = this.style;
		
		this.suggestionBox = new Box();
		this.suggestionBox.Visible = false;
		this.suggestionBox.Left = this.editbox.Left;
		this.suggestionBox.Width = 150f;
		this.suggestionBox.ClassStyle = this.style;
		this.suggestionBox.ElementStyle.Hover.Background = this.style.Background;

		this.AddElement(this.background);
		this.AddElement(this.button);
		this.AddElement(this.editbox);
		this.AddElement(this.editboxHelp);
		this.AddElement(this.scrollBox);
		this.AddElement(this.suggestionBox);
	}

	public override void Update()
	{
		Rect currentResolution = new Rect(0, 0, Screen.width, Screen.height);
		if (!currentResolution.Equals(this.resolution))
		{
			this.resolution = currentResolution;
			
			// update background, editbox, button positions
			this.background.Width = Screen.width;
			this.background.Height = Screen.height;

			float editboxMargin = this.editbox.Left;
			float elementTop = Screen.height - UnityEngine.Mathf.Max(this.editbox.Height, this.button.Height) - editboxMargin;
			float buttonWidth = this.button.Width;

			this.editbox.Top = elementTop;
			this.editbox.Width = Screen.width - buttonWidth - 3f * editboxMargin;
			
			if (this.editboxHelp.Visible)
			{
				this.editboxHelp.Top = elementTop;
				this.editboxHelp.Width = this.editbox.Width;
			}
			
			this.button.Top = elementTop;
			this.button.Left = Screen.width - buttonWidth - editboxMargin;
			
			this.scrollBox.Height = elementTop - scrollBox.Top * 2f;
			this.scrollBox.Width = Screen.width - this.scrollBox.Left * 2f;
			this.updateMessageLabels = true;
			
			this.updateFilterBoxPosition();
			
			this.suggestionBox.Top = elementTop - this.suggestionBox.Height;
		}
		
		if (this.filterBox.Visible)
		{
			// check filter
		}
		
		if (this.scrollToBottom)
		{
			this.scrollToBottom = false;
			this.scrollBox.VerticalScrollPosition = this.scrollBox.VerticalScrollMaximum;
		}
	}
	
	public override void OnRepaint()
	{
		if (this.updateMessageLabels)
		{
			this.updateMessageLabelPositions();
		}
	}
	
	# region filter
	private void updateFilterBox()
	{
//		bool update = false;
//		string[] loggerNames = OverlayManager.Instance.Console.Loggers();
//		Logger[] tempLoggers = new Logger[loggerNames.Length];
//		for (int i = 0; i < loggerNames.Length; i++)
//		{
//			tempLoggers[i] = OverlayManager.Instance.Console.GetLogger(loggerNames[i]);
//		}
	}
	
	private void updateFilterBoxPosition()
	{
		
	}
	# endregion
	
	# region text
	/// <summary>
	/// Warning: Uses CalcHeight which needs to be called in OnRepaint.
	/// Set updateMessageLabels to true which will make this method be called next repaint.
	/// </summary>
	private void updateMessageLabelPositions()
	{
		this.updateMessageLabels = false;
		
		this.scrollToBottom = (this.scrollBox.VerticalScrollPosition >= this.scrollBox.VerticalScrollMaximum);
		
		float top = 0;
		float width = this.scrollBox.Width - this.scrollBox.VerticalScrollBarStyle.Width.Value;
		float currentHeight;
		foreach(IElement elem in this.messageLabels)
		{
			elem.Top = top;
			elem.Width = width;
			currentHeight = elem.CalcHeight(width);
			elem.Height = currentHeight;
			top += currentHeight;
		}
		
		this.scrollBox.VerticalScrollBarStyle.ThumbHeight = Mathf.Max(30f, this.scrollBox.Height * this.scrollBox.Height / Mathf.Max(top, this.scrollBox.Height));
	}
	
	private Color getColorFromLevel(GUILib.Types.LoggerLevel level)
	{
		switch (level)
		{
			case GUILib.Types.LoggerLevel.Error:
				return Color.red;
			case GUILib.Types.LoggerLevel.Warn:
				return Color.yellow;
			default:
				return Color.white;
		}
	}
	# endregion
	
	
	# region command suggestion
	private void handleCommandSuggestion(KeyCode key)
	{
		if (this.suggestedCommands != null && this.suggestedCommands.Length > 0 && this.editbox.Text.Trim().IndexOf(' ') < 0)
		{
			bool changeCommand = false;
			if (key == KeyCode.UpArrow || (key == KeyCode.Tab && Event.current.control))
			{
				// prev command
				changeCommand = true;
				this.suggestedCommandIndex--;
				if (this.suggestedCommandIndex < 0 || this.suggestedCommandIndex >= this.suggestedCommands.Length)
				{
					this.suggestedCommandIndex = this.suggestedCommands.Length - 1;
				}
			}
			else
			{
				if (key == KeyCode.DownArrow || key == KeyCode.Tab)
				{
					// next command
					changeCommand = true;
					this.suggestedCommandIndex++;
					if (this.suggestedCommandIndex >= this.suggestedCommands.Length)
					{
						this.suggestedCommandIndex = 0;
					}
				}
			}
			
			if (changeCommand)
			{
				this.editbox.Text = this.suggestedCommands[this.suggestedCommandIndex] + ' ';
			}
		}
	}
	
	private void updateCommandSuggestion()
	{
		if (this.suggestedCommands.Length > 0)
		{
			float top = 0;
			for (int i = 0; i < this.suggestedCommands.Length && i < 10; i++)
			{
				if (i >= this.suggestionButtons.Count)
				{
					this.suggestionButtons.Add(new Button());
					this.suggestionButtons[i].Top = top;
					this.suggestionButtons[i].Width = this.suggestionBox.Width;
					this.suggestionButtons[i].Height = 20f;
					this.suggestionButtons[i].ElementStyle.TextColor = Color.white;
					this.suggestionButtons[i].ElementStyle.BackgroundPath = "empty";
					this.suggestionButtons[i].ElementStyle.PaddingLeft = this.suggestionButtons[i].ElementStyle.PaddingRight = 2;
					this.suggestionButtons[i].ElementStyle.Alignment = TextAnchor.MiddleLeft;
					this.suggestionButtons[i].Click += this.handleCommandSuggestionClick;
					
					this.suggestionBox.AddElement(this.suggestionButtons[i]);
				}
				
				this.suggestionButtons[i].Text = this.suggestedCommands[i];
				
				top += this.suggestionButtons[0].Height;
			}
			
			this.suggestionBox.Height = top;
			this.suggestionBox.Top = this.editbox.Top - this.suggestionBox.Height;
			this.suggestionBox.Visible = true;
		}
		else
		{
			this.suggestionBox.Visible = false;
		}
	}
	
	# endregion
	
	# region delegates
	private void toggleFilter(IElement src)
	{
		this.filterBox.Visible = true;
	}
	
	private void execute(IElement src)
	{
		GUILib.ConsoleCommandRegistry.Instance.ExecuteCommand(this.editbox.Text);
	}
	
	private bool handleKeyEvent(IElement src, KeyCode key)
	{
		if (key == KeyCode.Return || key == KeyCode.KeypadEnter)
		{
			OverlayManager.Instance.Console.Print("]] " + this.editbox.Text);
			if (!GUILib.ConsoleCommandRegistry.Instance.ExecuteCommand(this.editbox.Text))
			{
				// no command found
				string input = this.editbox.Text;
				int firstSpace = input.IndexOf(' ');
				if (firstSpace > -1)
				{
					string command = input.Substring(0, firstSpace);
					OverlayManager.Instance.Console.Print("Unknown command: \"" + command + "\"");
				}
			}
		}
		else
		{
			if (this.editbox.IsFocused)
			{
				this.handleCommandSuggestion(key);
			}
		}
		
		return true;
	}
	
	private void handleOnChange(IElement src)
	{
		string text = this.editbox.Text.Trim();
		if (text.IndexOf(' ') < 0)
		{
			this.suggestedCommands = ConsoleCommandRegistry.Instance.GetCommands(text);
			this.suggestedCommandIndex = 11; // initialize higher than the maximum commands shown
			this.updateCommandSuggestion();
		}
		else
		{
			this.suggestionBox.Visible = false;
		}
		
		if (!"".Equals(this.editbox.Text))
		{
			this.handleEditBoxClear(src);
		}
	}
	
	private void handleCommandSuggestionClick(IElement src)
	{
		this.editbox.Text = src.Text;
		OverlayManager.Instance.SetFocus(this.editbox);
	}
	
	private void handleEditBoxClear(IElement src)
	{
		if (this.editboxHelp.Visible)
		{
			this.editboxHelp.Visible= false;
		}
	}
	# endregion
	
	# region abstract methods
	/// <summary>
	/// Will be called if the data changed. 
	/// </summary>
	public override void NotifyDataChanged ()
	{
		ReadOnlyCollection<KeyValuePair<GUILib.Types.LoggerLevel, string>> messages = this.consoleData.Messages;
		
		// Display new message and remove old (if above limit).
		while(messageLabels.Count < messages.Count)
		{
			// initialize new label
			Label label = new Label();
			label.ElementStyle.WordWrap = true;
			
			messageLabels.Add(label);
			scrollBox.AddElement(label);
		}
		
		// update text and color
		for (int i = 0; i < messages.Count; i++)
		{
			messageLabels[i].Text = messages[i].Value;
			messageLabels[i].ElementStyle.TextColor = this.getColorFromLevel(messages[i].Key);
			messageLabels[i].Visible = true;
		}
		// hide unused labels
		for (int i = messages.Count; i < messageLabels.Count; i++)
		{
			messageLabels[i].Visible = false;
		}
		
		
		this.updateMessageLabels = true;
	}
	
	public override void Show()
	{
		this.suggestionBox.Visible = false;
		this.editbox.Text = "";
		OverlayManager.Instance.SetFocus(this.editbox);
	}
	
	#endregion
}