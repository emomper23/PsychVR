#ifndef MAINWINDOW_H
#define MAINWINDOW_H


#include <QMainWindow>
#include <cunitymap.h>
#include <cunityobject.h>
#include "editdialog.h"
namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
public slots:
    void saveFiles();
    void loadFiles();
    void editModel();

private:
    Ui::MainWindow *ui;
    CUnityMap * m_map;
    EditDialog * m_obj_settings;


};

#endif // MAINWINDOW_H
