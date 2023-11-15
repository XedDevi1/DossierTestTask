var itemId;

$(document).ready(function () {
  $.ajax({
    url: 'https://localhost:7282/api/dossier',
    type: 'GET',
    success: function (data) {
      console.log('Полученные данные:', data); // Выводим полученные данные
      var tree = createTree(data);
      $('#jstree_demo_div').append(tree);
      $('#jstree_demo_div').jstree();
    },
    error: function (error) {
      console.log('Error: ' + error);
    }
  });

  // Создаем меню с полями для вывода информации и кнопкой сохранить
  var infoMenu = $('<div id="infoMenu" style="display: none; position: absolute; right: 0; width: 50%; padding: 10px;">');
  infoMenu.append('<label for="sectionCodeField" style="color: rgb(17, 81, 142); font-size: 1.5em; float: left; margin-right: 10px;">Раздел:</label>');
  infoMenu.append('<input id="sectionCodeField" type="text" placeholder="SectionCode" style="width: 50%; height: 30px; margin-bottom: 10px;">');
  infoMenu.append('<label for="nameField" style="color: rgb(17, 81, 142); font-size: 2em; clear: both;">Название раздела:</label>');
  infoMenu.append('<textarea id="nameField" placeholder="Name" style="width: 90%; height: 60px; margin-bottom: 10px; overflow: auto; resize: none;"></textarea>'); // Добавлено свойство resize: none;
  infoMenu.append('<button id="saveButton" style="font-size: 1em; padding: 5px; margin-top: 10px;">Сохранить</button>');
  $('#jstree_demo_div').after(infoMenu);

  var idField = $('<input id="idField" type="hidden">');
  $('#infoMenu').append(idField);

  //обработчик клика
  $(document).on('click', '#jstree_demo_div li', function (e) {
    e.stopPropagation(); // Останавливаем всплытие события

    // Сжимаем область списка на половину по ширине к левому краю
    $('#jstree_demo_div').css({ width: '50%', float: 'left' });

    // Показываем меню с полями для вывода информации
    $('#infoMenu').show();

    // Заполняем поля информацией о выбранном пункте
    var itemData = $(this).text().split(' ');
    $('#sectionCodeField').val($(this).attr('data-section-code'));
    $('#nameField').val($(this).attr('data-name'));

    // Устанавливаем значение скрытого input равным id выбранного элемента
    itemId = $(this).attr('data-id');
    $('#idField').val(itemId);

    // Выводим id выбранного элемента на консоль
    console.log('Выбранный элемент:', $(this)); // Выводим выбранный элемент
    console.log('Выбранный ID: ' + itemId); // Выводим id выбранного элемента
  });

  // Обработчик клика по кнопке сохранить
  $('#saveButton').click(function () {
    var sectionCode = $('#sectionCodeField').val();
    var name = $('#nameField').val();

    // Получаем id выбранного узла
    var nodeId = $('#jstree_demo_div').jstree('get_selected')[0];

    console.log($('#jstree_demo_div').find('li.selected')); // Добавлено для отладки

    if (nodeId) {
      $.ajax({
        url: 'https://localhost:7282/api/dossier/' + itemId + '/Update',
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify({
          SectionCode: sectionCode,
          Name: name
        }),
        success: function () {
          alert('Данные успешно обновлены!');
        },
        error: function (jqXHR, textStatus, errorThrown) {
          alert('Ошибка при обновлении данных: ' + textStatus + ', ' + errorThrown);
        }
      });
    } else {
      console.log($('#jstree_demo_div').find('li.selected'));
      var nodeId = $('#jstree_demo_div').jstree('get_selected')[0];
      alert('Не выбран узел для обновления. Выбранный узел: ' + nodeId);
    }
  });


  function createTree(data) {
    var tree = $('<ul>');
    data.forEach(function (item) {
      var node = $('<li>')
        .text(item.sectionCode + ' ' + item.name)
        .attr('data-section-code', item.sectionCode)
        .attr('data-name', item.name)
        .attr('data-id', item.id);
      console.log($(node).attr('data-id')); // Выводим id текущего элемента
      if (item.children && item.children.length > 0) {
        node.append(createTree(item.children));
      }
      tree.append(node);
    });
    return tree;
  }


  $.contextMenu({
    selector: '#jstree_demo_div li',
    trigger: 'right',
    items: {
      "Добавить подчинённый узел": {
        name: "Добавить подчинённый узел",
        callback: function (key, options) {
          $.ajax({
            url: 'https://localhost:7282/api/dossier/' + itemId + '/Child',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
              parentId: itemId,
              orderNumber: 0,
              sectionCode: 'string',
              name: 'Новый узел'
            }),
            success: function () {
              alert('Узел добавлен.');
            },
            error: function () {
              alert('Ошибка при добавлении узла.');
            }
          });
        }
      },
      "Добавить строку после": {
        name: "Добавить строку после",
        callback: function (key, options) {
          $.ajax({
            url: 'https://localhost:7282/api/dossier/' + itemId + '/After',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
              parentId: itemId,
              orderNumber: 0,
              sectionCode: 'string',
              name: 'Новый узел'
            }),
            success: function () {
              alert('Строка добавлена после.');
            },
            error: function () {
              alert('Ошибка при добавлении строки после.');
            }
          });
        }
      },
      "Добавить строку перед": {
        name: "Добавить строку перед",
        callback: function (key, options) {
          $.ajax({
            url: 'https://localhost:7282/api/dossier/' + itemId + '/Before',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
              parentId: itemId,
              orderNumber: 0,
              sectionCode: 'string',
              name: 'Новый узел'
            }),
            success: function () {
              alert('Строка добавлена перед.');
            },
            error: function () {
              alert('Ошибка при добавлении строки перед.');
            }
          });
        }
      },
      "Удалить узел": {
        name: "Удалить узел",
        callback: function (key, options) {
          var nodeName = $(this).attr('data-name'); // предполагается, что у вас есть атрибут data-name
          var confirmationMessage = 'Удалить узел ' + itemId + '? Вы уверены?';
          if (confirm(confirmationMessage)) {
            $.ajax({
              url: 'https://localhost:7282/api/dossier/' + itemId,
              type: 'DELETE',
              success: function () {
                alert('Узел удален.');
              },
              error: function () {
                alert('Ошибка при удалении узла.');
              }
            });
          }
        }
      }
    }
  });
});
