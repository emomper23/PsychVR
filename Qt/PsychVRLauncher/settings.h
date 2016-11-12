#ifndef SETTINGS_H
#define SETTINGS_H

#include <QDialog>
#include <QColor>
#include <QColorDialog>

namespace Ui {
class settings;
}

class settings : public QDialog
{
    Q_OBJECT

public:
    explicit settings(QWidget *parent = 0);
    ~settings();
public slots:
    void changeSettings();
    void setupSettings(int userNum);
    void changeColor();

private:
    Ui::settings *ui;
    QColorDialog setSkin;
    QColor curCol;
};

#endif // SETTINGS_H
