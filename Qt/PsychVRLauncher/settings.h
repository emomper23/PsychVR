#ifndef SETTINGS_H
#define SETTINGS_H

#include <QDialog>

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

private:
    Ui::settings *ui;
};

#endif // SETTINGS_H
