USE Quiz
GO
INSERT INTO Theory
VALUES
('1 �������� 1939 �. ���������� ������������ �������� �� ������ �������� ������ ������� �����. �������������� 6 ������ ���, 
��� ������� ����� �� ����� ������ ���������� �������� � ������� �������� ��������. 
����� � ���������� ����� �������� ������ ����� ����������� ���������, �� ������� ����������� ��� �������� �������� ������� ������� 
� ���� ���������� �������� ������� ������ �� ��������� ����������.
����� ��������� ������ �������� ��� ���������� ��������� �����: �������� ����� ���� ���������� ������ ������ ���� ������ �����, 
�� �������� �� ����. ������ 1940 �. ����������� �������� �����, ��� ��������� ������������� ����� ��� �������� ��������. 
��� �� ������� ���� ����������� ���������� �������� ���������� ����� � ��-�����, ������� � ������� � �������� ������� ����� 
��� 350 �������� ����������� ������� �����. 22 ���� 1940 �. ������� �������������� � ����� �� �����. ������ 1941 �.
������ ����� ���� ������������ �������. ����������� ������ ������������ � ������.
')
GO
INSERT INTO Tests
VALUES
('������ ������� �����', NEWID(), 1, 1, 'https://mil.ru/winner_may/history/more.htm?id=11982000@cmsArticle','00:10:00', 5, 4)
GO
INSERT INTO Questions
output inserted.Question_code
VALUES
(1, '���� ������ ������ ������� �����'),
(1, '������ ��������� ������'),
(1, '������� ���� �����������'),
(1, '��� ��������� ���������� �������� ��������'),
(1, '���� ���������� �������� � 1944 ����')
GO
INSERT INTO Answers
VALUES
(1, '22.06.1941', 0),
(1, '22.06.1942', 0),
(1, '1.09.1941', 0),
(1, '1.09.1939', 1),
(2, '�������', 0),
(2, '������', 1),
(2, '�������', 0),
(2, '������������', 0),
(3, '8', 0),
(3, '18', 0),
(3, '28', 1),
(3, '38', 0),
(4, '���������', 0),
(4, '����������', 1),
(4, '������', 0),
(4, '�����', 0),
(5, '����������', 0),
(5, '�������', 0),
(5, '���������', 1),
(5, '���������', 0)
GO