using UnityEngine;
using System.Collections;

public class MessageContainer : MonoBehaviour {
	
	private MessageObject[] messageObjs;
	
	private int counter = 0;
	
	private const string TAB = "   ";
	
	public void initContainer (int messagesCount) {
		messageObjs = new MessageObject[messagesCount];
	}
	
	public void addMessageObject (MessageObject message) {
		messageObjs [counter] = message;
		counter++;
	}
	
	public MessageObject getFirstMessage () {
		return messageObjs [0];
	}
	
	public MessageObject getMessageObject (int index) {
		foreach (MessageObject msg in messageObjs) {
			if (msg.getMessageIndex() == index) {
				return msg;
			}
		}
		return null;
	}

	public class MessageObject {
		private int messageIndex;
		private Texture portrait;
		private string messageText;
		private ButtonObject[] buttons;
		private int btnCounter = 0;
		
		public MessageObject (int messageIndex, string messageText, int buttonsCount) {
			this.messageIndex = messageIndex;
			if (!messageText.StartsWith("<")) {
				loadPortrait(messageText.Substring(0, 5));
			}
			this.messageText = messageText.Substring(messageText.IndexOf("<")).Replace("<tab>", TAB);
			this.buttons = new ButtonObject[buttonsCount];
		}
		
		public void addButton (string btnText, string instruction, string[] instructionParams) {
			buttons [btnCounter] = new ButtonObject (btnText, instruction, instructionParams, btnCounter);
			btnCounter++;
		}
		
		public int getMessageIndex () {
			return messageIndex;
		}
		
		public string getMessageText () {
			return messageText;
		}
		
		public ButtonObject[] getButtons () {
			return buttons;
		}

		private void loadPortrait (string portraitName) {
			switch (portraitName) {
				case "ALIKA": portrait = Imager.alikaPortrait; break;
				case "ROKOT": portrait = Imager.rokotPortrait; break;
				default: Debug.Log("Неизвестный портрет"); break;
			}
		}

		public Texture getPortrait () {
			return portrait;
		}
	}
	
	public class ButtonObject {
		private string btnText;
		private InstructionType instructionType;
		private string[] instructParams;
		private int btnIndex;
		
		public ButtonObject(string btnText, string instruction, string[] instructParams, int btnIndex)
		{
			this.btnText = btnText;
			this.btnIndex = btnIndex;
			this.instructionType = getInstructionTypeByName(instruction);
			this.instructParams = instructParams;
		}
		
		public string getBtnText () {
			return btnText;
		}
		
		public InstructionType getInstructionType () {
			return instructionType;
		}
		
		public string[] getInstructionParams () {
			return instructParams;
		}
		
		public int getBtnIndex () {
			return btnIndex;
		}
	}
	
	public enum InstructionType {
		CLOSE, GOTO, SKIPTO, CHAT
	}
	
	private static InstructionType getInstructionTypeByName(string value)
	{
		switch (value)
		{
			case "close": return InstructionType.CLOSE;
			case "goto": return InstructionType.GOTO;
			case "skipto": return InstructionType.SKIPTO;
			case "chat": return InstructionType.CHAT;
			default: Debug.Log("Неизвестная инструкция"); return InstructionType.CLOSE;
		}
	}
}