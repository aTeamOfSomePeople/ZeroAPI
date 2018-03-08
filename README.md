# ZeroAPI
<h2>Всё сделано достаточно просто и тупо, местами очень тупо</h2>
<br>
<h1>User</h1>
Fields
<ul>
<li>int Id</li>
<li>string Name</li>
<li>string Avatar</li>
</ul>
Methods
<ul>
<li>static User GetUserInfo(int Id) <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbspПолучить базовую информацию о пользователе по его id</li>
<li>static int GetUserId(string login, string password) <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbspПолучает id пользователя</li>
<li>List<Chat> GetUserChats() <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
<li>static bool CreateUser(string Name, string Login, string Password, string Avatar = "")<br>
&nbsp&nbsp&nbsp&nbspСоздаёт пользователя, Avatar является не обязательным параметром</li>
<li>static bool SendMessage(int ChatId, string Text)<br>
&nbsp&nbsp&nbsp&nbspОтправляет сообщение, в тот чат, который скажешь</li>
</ul>
<br>
<h1>Message</h1>
Fields
<ul>
<li>int ChatId</li>
<li>int UserId</li>
<li>string Text</li>
<li>DateTime Date</li>
</ul>
Methods
<ul>
<li>void GetAttachmentsToMessage() <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
<li>void GetUser() <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
<li>void GetChat() <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
</ul>
<br>
<h1>Chat</h1>
Fields
<ul>
<li>int Id</li>
<li>string Name</li>
<li></li>
</ul>
Methods
<ul>
<li>static Chat GetChatInfo(int Id) <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
<li>List<User> GetUsers() <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
<li>List<Message> GetMessages()<br>
&nbsp&nbsp&nbsp&nbspВозвращает список всех сообщений из чата</li>
<li>bool CreateChat(string Name, ChatType Type, List<User> Users) <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
<li>bool AddUser(User user) <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
</ul>
<br>
<h1>Attachment</h1>
Fields
<ul>
<li></li>
</ul>
Methods
<ul>
<li>void GetAttachment() <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
<li>void GetMessage() <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
</ul>
