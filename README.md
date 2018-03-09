# ZeroAPI
<h2>Всё сделано достаточно просто и тупо, местами очень тупо</h2>
<br>
<h2>Подключение к проетку</h2>
<ol>
  <li>Скачиваем данный проект и запускаем в visual studio.</li>
  <li>Если адрес вашего сервера отличается от этого "http://localhost:64038/", то заходим в Properties>resources, меняем ServerUrl на нужный.</li>
  <li>Собираем проект.</li>
  <li>В нужном вам проекте добавляем ссылку на проект с библиотекой.</li>
  <li>Библиотека подключена, Вы замечательны!</li>
</ol>
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
 <li>static List&ltUser&gt FindUsers(string name)</br>
 &nbsp&nbsp&nbsp&nbspПолучить всех пользователей, имя или логин которых содержит данную строку.</li>
<li>static User GetUserInfo(int Id) <br>
&nbsp&nbsp&nbsp&nbspПолучить информацию о пользователе по его id (Всю кроме пароля).</li>
<li>static User GetUser(string login, string password) <br>
&nbsp&nbsp&nbsp&nbspПолучает полную информацию о пользователе.</li>
<li>List&ltChat&gt GetUserChats()<br>
&nbsp&nbsp&nbsp&nbspПолучить все чаты, в которых состоит пользователь.</li>
<li>static bool CreateUser(string Name, string Login, string Password)<br>
&nbsp&nbsp&nbsp&nbspСоздаёт пользователя.</li>
<li>static bool SendMessage(int ChatId, string Text, string[] Attachments = null)<br>
&nbsp&nbsp&nbsp&nbspОтправляет сообщение, в тот чат, который скажешь, по желанию с вложениями</li>
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
<li>List&ltAttachment&gt GetAttachmentsToMessage()<br>
&nbsp&nbsp&nbsp&nbspПолучить все вложения к сообщению</li>
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
<li>List&ltUser&gt GetUsers() <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
<li>List&ltMessage&gt GetMessages()<br>
&nbsp&nbsp&nbsp&nbspВозвращает список всех сообщений из чата.</li>
<li>bool CreateChat(string Name, ChatType Type, List&ltUser&gt Users)<br>
&nbsp&nbsp&nbsp&nbspСоздаёт чат с указанными пользователями.</li>
<li>bool AddUser(User user)<br>
&nbsp&nbsp&nbsp&nbspДобавляет пользователя к чату.</li>
</ul>
<br>
<h1>Attachment</h1>
Fields
<ul>
<li>int Id</li>
<li>int MessageId</li>
<li>string Link</li>
</ul>
Methods
<ul>
<li>void GetAttachment() <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
<li>void GetMessage() <b>[Не работает]</b><br>
&nbsp&nbsp&nbsp&nbsp</li>
</ul>
