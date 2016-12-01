#ifndef MAINWINDOW_H
#define MAINWINDOW_H
#define NUM_MAPS 3

#include <QMainWindow>
#include <cunitymap.h>
#include <cunityobject.h>
#include "editdialog.h"
#include "settings.h"

#include <QSlider>
#include <QButtonGroup>
#include <QFile>
#include <QJsonDocument>
#include <QJsonObject>
#include <QSignalMapper>
#include <QJsonValue>
#include <QByteArray>
#include <QJsonArray>

#include <QColor>
#include <QColorDialog>
#include <qcustomplot.h>




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
    void readIn();
    void showSettings();
    void tabChanged(int tab);
    void axisRangeChanged( const QCPRange &newRange, const QCPRange &oldRange);
    void axisRangeChanged2( const QCPRange &newRange, const QCPRange &oldRange);
    void axisRangeChanged3( const QCPRange &newRange, const QCPRange &oldRange);
    void setSceneMenu(int idx);
    void setSceneToggle();
    QString createRun();
    QJsonArray makeJson();
    void changeSettings();
    void changeColor();
    void changeFile();
    void changeSong();
    void switchTabs(int index);
    void switchScene(int index);

private:
    Ui::MainWindow *ui;
    CUnityMap ** m_map_list;
    QGraphicsScene * m_calm_scene;
    QGraphicsScene * m_speech_scene;
    std::vector<QGraphicsScene*> m_height_scenes;
    //QGraphicsScene * m_height_scene;
    EditDialog * m_obj_settings;
    CUnityMap * getMap();
    std::vector <QButtonGroup*> radioQs;
    settings * m_settings;
    QColorDialog setSkin;
    QColor curCol;
    QString powerpoint;
    QString song;
    void loadImage(QString file, QGraphicsView *view, QGraphicsScene **scene);
    void allocScene(QString file,QGraphicsView *view,QGraphicsScene** scene);
    void setScene(int idx);
};

#endif // MAINWINDOW_H
