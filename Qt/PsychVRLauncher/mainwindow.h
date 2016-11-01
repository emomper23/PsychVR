#ifndef MAINWINDOW_H
#define MAINWINDOW_H
#define NUM_MAPS 3

#include <QMainWindow>
#include <cunitymap.h>
#include <cunityobject.h>
#include "editdialog.h"

#include <QSlider>
#include <QButtonGroup>
#include <QFile>
#include <QJsonDocument>
#include <QJsonObject>
#include <QSignalMapper>
#include <QJsonValue>
#include <QByteArray>
#include <QJsonArray>

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
   enum scene_idx_t {FEAR_OF_HEIGHTS,SPEECH_ANXIETY,TERRAIN_GENERATION};
public slots:
    void saveFiles();
    void loadFiles();
    void editModel();
    void saveModel();
    void newModel();
    void launchScene();
    void SaveData();
    void changeUser(int userName);
    void initButtons();

private:
    Ui::MainWindow *ui;
    CUnityMap ** m_map_list;
    EditDialog * m_obj_settings;
    CUnityMap * getMap();
    std::vector <QButtonGroup*> radioQs;

};

#endif // MAINWINDOW_H
